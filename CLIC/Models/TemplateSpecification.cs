using System;
using System.Collections.Generic;
using System.Text;

namespace CLIC.Models
{
    public class TemplateSpecification
    {
        public List<CLIQuestion> Questions { get; set; }

        public bool IsESNext { get; set; }
        public bool IsTypeScript { get; set; }
        public bool IsCustomSetup { get; set; }

        public string Name { get; set; }

        public ModuleLoaderTypes ModuleLoader { get; set; }
        public HttpTypes HttpType { get; set; }
        public PlatformTypes PlatformType { get; set; }
        public TranspilerTypes Transpiler { get; set; }
        public MinificationLevel Minification { get; set; }
        public CSSProcessorTypes CSSProcessing { get; set; }
        public UnitTestRunnerTypes UnitTestRunner { get; set; }
        public IntegrationTestingTypes IntegrationTesting { get; set; }
        public IDETypes CodeEditor { get; set; }

        public bool CreateNow { get; set; }
        public bool InstallDependecies { get; set; }

        public TemplateSpecification()
        {
            Questions = GetQuestions();
        }

        List<CLIQuestion> GetQuestions()
        {
            var qList = new List<CLIQuestion>();

            var q0 = new CLIQuestion { Id = 0, Text = "Enter project name:" };
            q0.QuestionType = CLIQuestionType.AlphabeticInput;

            var q1 = new CLIQuestion { Id = 1, Text = "Which module loader / bundler would you like to use?" };
            q1.QuestionType = CLIQuestionType.NumericSelection;
            q1.Options.Add(@"Webpack (Default)
   A powerful and popular bundler for JavaScript");
            q1.Options.Add(@"CLI's built-in bundler with Alameda
   Alameda is a modern version of RequireJS using promises and native es6 features (modern browsers
   only).");
            q1.Options.Add(@"CLI's built-in bundler with SystemJS
   SystemJS is Dynamic ES module loader, the most versatile module loader for JavaScript");

            var q2 = new CLIQuestion { Id = 2, Text = "Which HTTP Protocol do you wish the outputted webpack bundle to be optimised for?" };
            q2.QuestionType = CLIQuestionType.NumericSelection;
            q2.Options.Add(@"HTTP/1.1 (Default)
   The legacy HTTP/1.1 protocol, max 6 parallel requests/connections");
            q2.Options.Add(@"HTTP/2
   The modern HTTP/2 Protocol, uses request multiplexing over a single connection");

            var q3 = new CLIQuestion { Id = 3, Text = "What platform are you targeting?" };
            q3.QuestionType = CLIQuestionType.NumericSelection;
            q3.Options.Add(@"Web (Default)
   The default web platform setup");
            q3.Options.Add(@"ASP.NET Core
   A powerful, patterns-based way to build dynamic websites with .NET");

            var q4 = new CLIQuestion { Id = 4, Text = "What transpiler would you like to use?" };
            q4.QuestionType = CLIQuestionType.NumericSelection;
            q4.Options.Add(@"Babel (Default)
   An open source, standards-compliant ES2015 and ESNext transpiler");
            q4.Options.Add(@"TypeScript
   An open source, ESNext superset that adds optional strong typing");

            var q5 = new CLIQuestion { Id = 5, Text = "How would you like to setup your template? (markup minification)" };
            q5.QuestionType = CLIQuestionType.NumericSelection;
            q5.Options.Add(@"Default (Default)
   No markup processing");
            q5.Options.Add(@"Minimum Minification
   Removes comments and whitespace between block level elements such as div, blockquote, p, header,
   footer ...etc");
            q5.Options.Add(@"Maximum Minification
   Removes comments, script & link element [type] attributes and all whitespace between all elements.
   Also remove attribute quotes where possible. Collapses boolean attributes");

            var q6 = new CLIQuestion { Id = 6, Text = "What CSS processor would you like to use?" };
            q6.QuestionType = CLIQuestionType.NumericSelection;
            q6.Options.Add(@"None (Default)
   Use standard CSS with no pre-processor");
            q6.Options.Add(@"Less
   Extends the CSS language, adding features that allow variables, mixins, functions and many other
   techniques");
            q6.Options.Add(@"Sass
   A mature, stable, and powerful professional grade CSS extension");
            q6.Options.Add(@"Stylus
   Expressive, dynamic and robust CSS");
            q6.Options.Add(@"PostCSS
   A tool for transforming CSS with JavaScript");

            var q7 = new CLIQuestion { Id = 7, Text = "What unit test runners would you like to use?" };
            q7.QuestionType = CLIQuestionType.NumericSelection;
            q7.Options.Add(@"None
   Skip testing. My code is always perfect anyway");
            q7.Options.Add(@"Karma
   Configure your app with Karma and Jasmine");
            q7.Options.Add(@"Jest
   Configure your app with Jest and Jasmine");

            var q8 = new CLIQuestion { Id = 8, Text = "Would you like to configure integration testing?" };
            q8.QuestionType = CLIQuestionType.NumericSelection;
            q8.Options.Add(@"No (Default)
   Skip testing. My code is always perfect anyway");
            q8.Options.Add(@"Protractor
   Configure your app with Protractor");

            var q9 = new CLIQuestion { Id = 9, Text = "What is your default code editor?" };
            q9.QuestionType = CLIQuestionType.NumericSelection;
            q9.Options.Add(@"Visual Studio Code (Default)
   Code editing. Redefined. Free. Open source. Runs everywhere.");
            q9.Options.Add(@"Atom
   A hackable text editor for the 21st Century.");
            q9.Options.Add(@"Sublime
   A sophisticated text editor for code, markup and prose.");
            q9.Options.Add(@"WebStorm
   A lightweight yet powerful IDE, perfectly equipped for complex client-side development.");
            q9.Options.Add(@"Rider
   Incredible .NET IDE with the power of ReSharper! (new)");
            q9.Options.Add(@"None of the Above
   Do not configure any editor-specific options.");

            //Summary is displayed

            var q10 = new CLIQuestion { Id = 10, Text = "Would you like to create this project?" };
            q10.QuestionType = CLIQuestionType.NumericSelection;
            q10.Options.Add(@"Yes");
            q10.Options.Add(@"No");

            var q11 = new CLIQuestion { Id = 11, Text = "Would you like to install the project dependencies?" };
            q11.QuestionType = CLIQuestionType.NumericSelection;
            q11.Options.Add(@"Yes");
            q11.Options.Add(@"No");

            //Finished summary

            //Add all questions to list
            qList.Add(q0);
            qList.Add(q1);
            qList.Add(q2);
            qList.Add(q3);
            qList.Add(q4);
            qList.Add(q5);
            qList.Add(q6);
            qList.Add(q7);
            qList.Add(q8);
            qList.Add(q9);
            qList.Add(q10);
            qList.Add(q11);

            return qList;
        }

        public void SetChoice(int index, string selection)
        {
            Questions[index].Answer = selection;
        }

        public void ProcessChoices()
        {
            foreach (var q in Questions)
                ProcessSpecificationChoice(q);
        }

        public void ProcessSpecificationChoice(CLIQuestion question)
        {
            // ModuleLoaderTypes        ModuleLoader
            // HttpTypes                HttpType 
            // PlatformTypes            PlatformType 
            // TranspilerTypes          Transpiler
            // MinificationLevel        Minification 
            // CSSProcessorTypes        CSSProcessing 
            // UnitTestRunnerTypes      UnitTestRunner 
            // IntegrationTestingTypes  IntegrationTesting
            // IDETypes                 CodeEditor

            switch (question.Id) {
                case 0:
                    Name = question.Answer.Trim();
                    break;
                case 1:
                    ModuleLoader = Enum.Parse<ModuleLoaderTypes>(question.Answer);
                    break;
                case 2:
                    HttpType = Enum.Parse<HttpTypes>(question.Answer);
                    break;
                case 3:
                    PlatformType = Enum.Parse<PlatformTypes>(question.Answer);
                    break;
                case 4:
                    Transpiler = Enum.Parse<TranspilerTypes>(question.Answer);
                    break;
                case 5:
                    Minification = Enum.Parse<MinificationLevel>(question.Answer);
                    break;
                case 6:
                    CSSProcessing = Enum.Parse<CSSProcessorTypes>(question.Answer);
                    break;
                case 7:
                    UnitTestRunner = Enum.Parse<UnitTestRunnerTypes>(question.Answer);
                    break;
                case 8:
                    IntegrationTesting = Enum.Parse<IntegrationTestingTypes>(question.Answer);
                    break;
                case 9:
                    CodeEditor = Enum.Parse<IDETypes>(question.Answer);
                    break;
                case 10:
                    CreateNow = question.Answer == "1";
                    break;
                case 11:
                    InstallDependecies = question.Answer == "1";
                    break;
            }
        }

    }


    public enum ModuleLoaderTypes
    {
        Webpack = 0,
        RequireJS = 1, 
        Alameda = 2,
        SystemJS = 3
    }

    public enum HttpTypes
    {
        Http1_1 = 0,
        Http2 = 1
    }

    public enum PlatformTypes
    {
        Web = 0,
        ASP_NET_CORE = 1
    }

    public enum TranspilerTypes
    {
        Babel = 0,
        TypeScript = 1
    }

    public enum MinificationLevel
    {
        None = 0,
        Minimum = 1,
        Maximum = 2
    }

    public enum CSSProcessorTypes
    {
        None = 0,
        Less = 1,
        Sass = 2,
        Stylus = 3,
        PostCSS = 4
    }

    public enum UnitTestRunnerTypes
    {
        None = 0,
        Karma = 1,
        Jest = 2
    }

    public enum IntegrationTestingTypes
    {
        None = 0,
        Protractor = 1
    }

    public enum IDETypes
    {
        None = 5,
        VisualStudioCode = 0,
        Atom = 1,
        Sublime = 2,
        Webstorm = 3,
        Rider = 4
    }

}
