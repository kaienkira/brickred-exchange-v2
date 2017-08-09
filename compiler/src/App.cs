using Brickred.Exchange.Compiler;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class App
{
    private static void PrintUsage()
    {
        Console.Error.WriteLine("brickred exchange compiler");
        Console.Error.WriteLine(string.Format(
            "usage: {0} " +
            "-f <protocol_file> " +
            "-l <language>",
            AppDomain.CurrentDomain.FriendlyName));
        Console.Error.WriteLine(
            "    [-o <output_dir>]");
        Console.Error.WriteLine(
            "    [-I <search_path>]");
        Console.Error.WriteLine(
            "    [-n <new_line_type>] (unix|dos) default is unix");
        Console.Error.WriteLine(
            "language supported: cpp php csharp");
    }

    public static int Main(string[] args)
    {
        string optProtoFilePath = "";
        string optLanguage = "";
        string optOutputDir = "";
        List<string> optSearchPath = new List<string>();
        string optNewLineType = "";

        // parse command line options
        {
            OptionSet options = new OptionSet();
            options.Add("f=", v => optProtoFilePath = v);
            options.Add("l=", v => optLanguage = v);
            options.Add("o=", v => optOutputDir = v);
            options.Add("I=", v => optSearchPath.Add(v));
            options.Add("n=", v => optNewLineType = v);

            try {
                options.Parse(args);
            } catch (OptionException e) {
                Console.Error.WriteLine(e.Message);
                return 1;
            }
        }


        // check command line options
        if (optProtoFilePath == "" ||
            optLanguage == "") {
            PrintUsage();
            return 1;
        }
        if (optOutputDir == "") {
            optOutputDir = ".";
        }

        if (File.Exists(optProtoFilePath) == false) {
            Console.Error.WriteLine(string.Format(
                "error: can not find protocol file `{0}`",
                optProtoFilePath));
            return 1;
        }
        if (Directory.Exists(optOutputDir) == false) {
            Console.Error.WriteLine(string.Format(
                "error: can not find output directory `{0}`",
                optOutputDir));
            return 1;
        }

        using (ProtocolParser parser = new ProtocolParser()) {
            if (parser.Parse(optProtoFilePath, optSearchPath) == false) {
                return 1;
            }

            BaseCodeGenerator generator = null;
            if (optLanguage == "cpp") {
                generator = new CppCodeGenerator();
            } else if (optLanguage == "php") {
                generator = new PhpCodeGenerator();
            } else if (optLanguage == "csharp") {
                generator = new CSharpCodeGenerator();
            } else {
                Console.Error.WriteLine(string.Format(
                    "error: language `{0}` is not supported",
                    optLanguage));
                return 1;
            }

            using (generator) {
                BaseCodeGenerator.NewLineType newLineType =
                    BaseCodeGenerator.NewLineType.Unix;
                if (optNewLineType == "dos") {
                    newLineType = BaseCodeGenerator.NewLineType.Dos;
                }

                if (generator.Generate(parser.Descriptor,
                        optOutputDir, newLineType) == false) {
                    return 1;
                }
            }
        }

        return 0;
    }
}
