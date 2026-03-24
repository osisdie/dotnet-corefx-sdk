using System;
using System.Runtime.CompilerServices;

namespace CoreFX.Abstractions.Tests;

internal static class TestModuleInitializer
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        Environment.SetEnvironmentVariable("COREFX_API_NAME", "test-api");
    }
}
