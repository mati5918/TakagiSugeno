using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Wrappers;
using TakagiSugeno.Model;
using Xunit;

namespace TakagiSugent.Tests
{
    public class MembershipFunctionsTests
    {
        public MembershipFunctionsTests()
        {
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Theory]
        [InlineData(2,1)]
        [InlineData(1.5, 0.5)]
        [InlineData(-2, 0)]
        public void MembershipDegreeTest(double value, double expected)
        {
            //Assert.Equal(5, Add(2, 2));
            string jsonStr = @"{a: 1, b: 2, c:3}";
            Variable var = new Variable { Data = jsonStr, Type = VariableType.Triangle };
            VariableWrapper variable = new VariableWrapper(var);
            double res = variable.MembershipFunction.CalcMembership(value);
            Assert.Equal(expected, res);
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
