﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Lava;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Rock.Tests.Integration.Lava
{
    /// <summary>
    /// Tests for parallel execution and multi-threading issues.
    /// </summary>
    [TestClass]
    public class ParallelExecutionTests : LavaIntegrationTestBase
    {
        [TestMethod]
        public void ParallelExecution_ShortcodeWithParameters_ResolvesParameterCorrectly()
        {
            var shortcodeTemplate = @"
Font Name: {{ fontname }}
Font Size: {{ fontsize }}
Font Bold: {{ fontbold }}
";

            var input = @"
{[ shortcodetest fontname:'Arial' fontsize:'{{ fontsize }}' fontbold:'true' ]}
{[ endshortcodetest ]}
";

            var expectedOutput = @"
Font Name: Arial
Font Size: <?>
Font Bold: true
";

            // Create a new test shortcode.
            var shortcodeDefinition = new DynamicShortcodeDefinition();

            shortcodeDefinition.ElementType = LavaShortcodeTypeSpecifier.Block;
            shortcodeDefinition.TemplateMarkup = shortcodeTemplate;
            shortcodeDefinition.Name = "shortcodetest";
            shortcodeDefinition.Parameters = new Dictionary<string, string> { { "fontname", "Arial" }, { "fontsize", "0" }, { "fontbold", "true" } };

            TestHelper.ExecuteForActiveEngines( ( engine ) =>
            {
                if ( engine.EngineType == LavaEngineTypeSpecifier.RockLiquid )
                {
                    TestHelper.DebugWriteRenderResult( engine.EngineType, "(Ignored)", "(Ignored)" );
                    return;
                }

                engine.RegisterShortcode( shortcodeDefinition.Name, ( shortcodeName ) => { return shortcodeDefinition; } );

                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 100 };

                Parallel.For( 1, 1000, parallelOptions, ( x ) =>
                {
                    var context = new LavaDataDictionary()
                    {
                    { "fontsize", x },
                    };
                    context["fontsize"] = x;

                    var options = new LavaTestRenderOptions() { MergeFields = context, Wildcards = new List<string> { "<?>" } };

                    TestHelper.AssertTemplateOutput( engine.EngineType, expectedOutput, input, options );
                } );
            } );
        }

        [TestMethod]
        public void ParallelExecution_ShortcodeWithChildItems_EmitsCorrectHtml()
        {
            var shortcodeTemplate = @"
Parameter 1: {{ parameter1 }}
Parameter 2: {{ parameter2 }}
Items:
{%- for item in items -%}
{{ item.title }} - {{ item.content }}
{%- endfor -%}
";

            // Create a new test shortcode.
            var shortcodeDefinition = new DynamicShortcodeDefinition();

            shortcodeDefinition.ElementType = LavaShortcodeTypeSpecifier.Block;
            shortcodeDefinition.TemplateMarkup = shortcodeTemplate;
            shortcodeDefinition.Name = "shortcodetest";
            shortcodeDefinition.Parameters = new Dictionary<string, string> { { "parameter1", "value1" }, { "parameter2", "value2" } };

            var input = @"
***
Iteration: {{ iteration }}
***
{[ shortcodetest ]}

    [[ item title:'Panel 1' ]]
        Panel 1 content.
    [[ enditem ]]
    
    [[ item title:'Panel 2' ]]
        Panel 2 content.
    [[ enditem ]]
    
    [[ item title:'Panel 3' ]]
        Panel 3 content.
    [[ enditem ]]

{[ endshortcodetest ]}
";

            var expectedOutput = @"
***
Iteration: <?>
***
Parameter 1: value1
Parameter 2: value2
Items:
Panel 1 - Panel 1 content.
Panel 2 - Panel 2 content.
Panel 3 - Panel 3 content.
";

            expectedOutput = expectedOutput.Replace( "``", @"""" );

            TestHelper.ExecuteForActiveEngines( ( engine ) =>
            {
                if ( engine.EngineType == LavaEngineTypeSpecifier.RockLiquid )
                {
                    TestHelper.DebugWriteRenderResult( engine.EngineType, "(Ignored)", "(Ignored)" );
                    return;
                }

                engine.RegisterShortcode( shortcodeDefinition.Name, ( shortcodeName ) => { return shortcodeDefinition; } );

                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };

                Parallel.For( 0, 1000, parallelOptions, ( x ) =>
                {
                    var context = new LavaDataDictionary();
                    context["iteration"] = x;

                    var options = new LavaTestRenderOptions() { MergeFields = context, Wildcards = new List<string> { "<?>" } };

                    TestHelper.AssertTemplateOutput( engine.EngineType, expectedOutput, input, options );
                } );
            } );

        }

    }
}