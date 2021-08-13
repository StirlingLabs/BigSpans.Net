using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Bogus.Platform;
using Iced.Intel;
using NUnit.Framework;
using StirlingLabs.Utilities;
using StirlingLabs.Utilities.Magic;

namespace StirlingLabs.BigSpans.Tests
{
    [ExcludeFromCodeCoverage]
    public static class JitExamTests
    {
        private const BindingFlags AllDeclared = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance;

        [DatapointSource]
        public static IEnumerable<DecompileMethod> Values
        {
            get {

                static unsafe DecompileMethod _0()
                {
                    // create function pointer
                    var d = (delegate*<bool>)&IfType<byte>.Is<bool>;
                    // invoke it to jit it
                    d();
                    // decompile it
                    return new(d, "StirlingLabs.Utilities.Magic.IfType<bool>.Is<bool>");
                }

                yield return _0();

                static unsafe DecompileMethod _1()
                {
                    // create function pointer
                    var d = (delegate*<bool>)&IfType<bool>.Is<bool>;
                    // invoke it to jit it
                    d();
                    // decompile it
                    return new(d, "StirlingLabs.Utilities.Magic.IfType<bool>.Is<bool>");
                }

                yield return _1();

                static unsafe DecompileMethod _2()
                {
                    // create function pointer
                    var d = (delegate*<bool>)&IfType<bool>.Is<byte>;
                    // invoke it to jit it
                    d();
                    // decompile it
                    return new(d, "StirlingLabs.Utilities.Magic.IfType<bool>.Is<bool>");
                }

                yield return _2();

                foreach (var mi in typeof(UnmanagedMemory).GetMethods(AllDeclared))
                    yield return mi;

                foreach (var mi in typeof(BigSpan).GetMethods(AllDeclared))
                    yield return mi;

                foreach (var mi in typeof(IfType<bool>).GetMethods(AllDeclared))
                    yield return mi;

                foreach (var mi in typeof(IfType<byte>).GetMethods(AllDeclared))
                    yield return mi;

            }
        }

        [SuppressMessage("Maintainability", "CA1502", Justification = "It's just a hack to get a preview of JIT compiled code")]
        [Explicit]
        [Theory]
        public static unsafe void QuickAsmPeek(DecompileMethod dm)
        {
            if (dm.Pointer == default)
                throw new InconclusiveException("Null pointer.");

            var p = dm.Pointer;

            var formatter = new IntelFormatter
            {
                Options =
                {
                    DigitSeparator = "`",
                    FirstOperandCharIndex = 10
                }
            };

            var m = (byte*)p;
            var u = new UnsafeMemoryViewAsStream(m);
            var reader = new StreamCodeReader(u);
            var ip = (ulong)p;
            var decoder = Decoder.Create(sizeof(nuint) * 8, reader, ip);
            var output = new StringOutput();
            var decodeLimit = 1000;
            Instruction ins = default;
            var seen = new HashSet<ulong> { u.Value };
            for (;;)
            {
                var prevIns = ins;
                if (decodeLimit-- <= 0) break;
                decoder.Decode(out ins);
                if (ins.IsInvalid || ins.IsPrivileged) break;
                formatter.Format(ins, output);
                TestContext.Write(ins.IP.ToString("X16"));
                TestContext.Write(" ");
                var instrLen = (uint)ins.Length;
                for (var i = 0uL; i < instrLen; i++)
                    TestContext.Write(((byte*)ins.IP)[i].ToString("X2"));
                var missingBytes = 10 - instrLen;
                for (var i = 0uL; i < missingBytes; i++)
                    TestContext.Write("  ");
                TestContext.Write(" ");
                TestContext.WriteLine(output.ToStringAndReset());

                bool CheckForEndOfValidCode()
                {
                    // release padding
                    if (IsPadding(u, 0x00))
                    {
                        TestContext.WriteLine("/* zero padding follows */");
                        return true;
                    }
                    // interrupt padding
                    if (IsPadding(u, 0xCC))
                    {
                        TestContext.WriteLine("/* breakpoint padding follows */");
                        return true;
                    }
                    if (u.Pointer[0] == 0x00 && u.Pointer[1] == 0x19 && u.Pointer[2] == 0x0A && u.Pointer[3] == 0x05)
                    {
                        TestContext.WriteLine("/* metadata follows */");
                        return true;
                    }
                    if (u.Pointer[0] == 0x19 && u.Pointer[1] == 0x05 && u.Pointer[2] == 0x02)
                    {
                        TestContext.WriteLine("/* metadata follows */");
                        return true;
                    }
                    if (u.Pointer[0] == 0x00 && u.Pointer[1] == 0x19 && u.Pointer[2] == 0x05 && u.Pointer[3] == 0x02)
                    {
                        TestContext.WriteLine("/* metadata follows */");
                        return true;
                    }
                    if (u.Pointer[0] == 0x19 && u.Pointer[1] == 0x04 && u.Pointer[2] == 0x01)
                    {
                        TestContext.WriteLine("/* metadata follows */");
                        return true;
                    }
                    if (u.Pointer[0] == 0x19 && u.Pointer[1] == 0x01 && u.Pointer[2] == 0x01 && u.Pointer[3] == 0x00)
                    {
                        TestContext.WriteLine("/* metadata follows */");
                        return true;
                    }
                    TestContext.WriteLine($"/* {u.Pointer[0]:X2} {u.Pointer[1]:X2} {u.Pointer[2]:X2} {u.Pointer[3]:X2} ... */");
                    return false;
                }

                if (ins.FlowControl == FlowControl.UnconditionalBranch)
                {
                    if (ins.IsJmpShortOrNear)
                    {
                        var target = ins.NearBranchTarget;
                        TestContext.WriteLine("----------------");
                        if (!seen.Add(target))
                        {
                            TestContext.WriteLine("----- LOOP -----");
                            TestContext.WriteLine("----------------");

                            if (CheckForEndOfValidCode())
                                goto ExitDisassembly;
                            continue;
                        }
                        decoder.IP = target;
                        u.Value = (nuint)decoder.IP;
                    }
                    else
                        throw new NotImplementedException(ins.ToString());
                    if (CheckForEndOfValidCode())
                        goto ExitDisassembly;
                }
                else if (ins.FlowControl == FlowControl.IndirectBranch)
                {
                    if (ins.IsJmpNearIndirect)
                    {
                        if (sizeof(nuint) == 8 && prevIns.OpCode.Code == Code.Mov_r64_imm64)
                        {
                            if (ins.Op0Register == prevIns.Op0Register)
                            {
                                var target = prevIns.GetImmediate(1);
                                TestContext.WriteLine("----------------");
                                if (!seen.Add(target))
                                {
                                    TestContext.WriteLine("----- LOOP -----");
                                    TestContext.WriteLine("----------------");

                                    if (CheckForEndOfValidCode())
                                        goto ExitDisassembly;
                                    continue;
                                }
                                decoder.IP = target;
                                u.Value = (nuint)decoder.IP;
                            }
                        }
                        else if (sizeof(nuint) == 4 && prevIns.OpCode.Code == Code.Mov_r32_imm32)
                            if (ins.Op0Register == prevIns.Op0Register)
                            {
                                var target = prevIns.GetImmediate(1);
                                TestContext.WriteLine("----------------");
                                if (!seen.Add(target))
                                {
                                    TestContext.WriteLine("----- LOOP -----");
                                    TestContext.WriteLine("----------------");

                                    if (CheckForEndOfValidCode())
                                        goto ExitDisassembly;
                                    continue;
                                }
                                decoder.IP = target;
                                u.Value = (nuint)decoder.IP;
                            }
                    }
                    else
                        throw new NotImplementedException();
                }
                else if (ins.FlowControl == FlowControl.Return)
                {
                    if (CheckForEndOfValidCode())
                        goto ExitDisassembly;
                }
                else

                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (ins.Code)
                    {
                        case Code.Int3: {
                            // aligned break is not padding
                            var o = u.Value - 1;
                            var a = o & 3;
                            //TestContext.WriteLine($"/* checking if {o:X} is aligned: {a} */");
                            if (a == 0)
                                goto ExitDisassembly;
                            break;
                        }
                        case Code.Add_rm8_r8: {
                            //TestContext.WriteLine($"/* {ins.MemoryBase} : {ins.Op1Register}*/");

                            // ReSharper disable once ConvertIfStatementToSwitchStatement
                            if (ins.MemoryBase == Register.RAX && ins.Op1Register == Register.AL
                                && (prevIns.Code == Code.Int3 || prevIns.FlowControl == FlowControl.Return))
                            {
                                TestContext.WriteLine("/* probably not code */");
                                goto ExitDisassembly;
                            }
                            if (ins.MemoryBase == Register.RCX && ins.Op1Register == Register.BL
                                && (prevIns.Code == Code.Int3 || prevIns.FlowControl == FlowControl.Return))
                            {
                                TestContext.WriteLine("/* probably not code */");
                                goto ExitDisassembly;
                            }
                            break;
                        }

                    }
            }
            ExitDisassembly:
            { }
        }

        private static unsafe bool IsPadding(UnsafeMemoryViewAsStream u, byte v)
        {
            var p = u.Pointer;
            var l = (nuint)p & 3;
            //TestContext.WriteLine($"/* checking for {l} bytes of {v:X} */");
            if (l == 0) return false;
            for (var i = 0u; i < l; ++i)
            {
                var b = p[i];
                //TestContext.WriteLine($"/* checking byte {i}: {b:X} */");
                if (b != v)
                    return false;
            }
            return true;
        }
    }
}
