using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Wrappers;
using Xunit;

namespace TakagiSugent.Tests
{
    public class RulesTests
    {
        /*[Fact]
        public void RuleMembershipDegressTest()
        {
            //IServiceProvider provider = 
            
            // Input A - id 1, variables A1 - id 1; A2 - id 2
            // Input B - id 2, variables B-1 - id 3; B1 - id 4; B3 - id 5
            Rule rule = new Rule { RuleElements = new List<RuleElement>() };
            rule.RuleElements.Add(new RuleElement { InputOutputId = 1, VariableId = 2 });
            rule.RuleElements.Add(new RuleElement { InputOutputId = 2, VariableId = 3 });

            Variable var1 = new Variable { Data = @"{a: 0, b: 2, c: 4}", Type = VariableType.Triangle, VariableId = 2, InputOutputId = 1 };
            InputVariableWrapper A1 = new InputVariableWrapper(var1);

            Variable var2 = new Variable { Data = @"{a: -3, b: -1, c: 1}", Type = VariableType.Triangle, VariableId = 3, InputOutputId = 2 };
            InputVariableWrapper B1 = new InputVariableWrapper(var2);

            Variable var3 = new Variable { Data = @"{a: -1, b: 1, c: 3}", Type = VariableType.Triangle, VariableId = 4, InputOutputId = 2 };
            InputVariableWrapper B2 = new InputVariableWrapper(var3);

            OutputCalculator calc = new OutputCalculator()
            {
                //Variables = new List<InputVariableWrapper> { A1, B1, B2 },
                InputValues = new Dictionary<int, double> { {1, 1.5 },{2, 0.5 } }
            };

            List<MembershipDegree> res = calc.CalcRuleMembershipDegrees(rule);

            Assert.Equal(0.75, res[0].Value);
            Assert.Equal(0.25, res[1].Value);

        }

        [Fact]
        public void RuleMembershipDegressExceptionTest()
        {
            // Input A - id 1, variables A1 - id 1; A2 - id 2
            // Input B - id 2, variables B-1 - id 3; B1 - id 4; B3 - id 5
            Rule rule = new Rule { RuleElements = new List<RuleElement>() };
            rule.RuleElements.Add(new RuleElement { InputOutputId = 1, VariableId = 2 });
            rule.RuleElements.Add(new RuleElement { InputOutputId = 5, VariableId = 3 });

            Variable var1 = new Variable { Data = @"{a: 0, b: 2, c: 4}", Type = VariableType.Triangle, VariableId = 2, InputOutputId = 1 };
            InputVariableWrapper A1 = new InputVariableWrapper(var1);

            OutputCalculator calc = new OutputCalculator()
            {
                //Variables = new List<InputVariableWrapper> { A1 },
                InputValues = new Dictionary<int, double> { { 1, 1.5 }, { 2, -1 } }
            };

            Exception ex = Assert.Throws<Exception>(() => calc.CalcRuleMembershipDegrees(rule));
            Assert.Equal("Variable not found", ex.Message);

        }*/

    }
}
