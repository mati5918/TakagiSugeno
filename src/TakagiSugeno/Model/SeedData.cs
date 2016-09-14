using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TakagiSugenoDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<TakagiSugenoDbContext>>()))
            {
                /*if (context.Systems.Any())
                {
                    return;   // DB has been seeded
                }
                TSSystem system = new TSSystem { Name = "Testowy" };
                context.Systems.Add(system);
                context.SaveChanges();

                InputOutput input1 = new InputOutput { Type = IOType.Input, Name = "X", TSSystemId = system.TSSystemId };
                InputOutput input2 = new InputOutput { Type = IOType.Input, Name = "Y", TSSystemId = system.TSSystemId };

                context.InputsOutputs.Add(input1);
                context.InputsOutputs.Add(input2);
                context.SaveChanges();
                
                Variable x1 = new Variable { Name = "A0", Type = VariableType.Triangle, InputOutputId = input1.InputOutputId, Data = @"{a: -2, b: 0, c: 2}" };
                Variable x2 = new Variable { Name = "A2", Type = VariableType.Triangle, InputOutputId = input1.InputOutputId, Data = @"{a: 0, b: 2, c: 4}" };

                Variable y1 = new Variable { Name = "B-1", Type = VariableType.Triangle, InputOutputId = input2.InputOutputId, Data = @"{a: -3, b: -1, c: 1}" };
                Variable y2 = new Variable { Name = "B1", Type = VariableType.Triangle, InputOutputId = input2.InputOutputId, Data = @"{a: -1, b: 1, c: 3}" };
                Variable y3 = new Variable { Name = "B3", Type = VariableType.Triangle, InputOutputId = input2.InputOutputId, Data = @"{a: 1, b: 3, c: 5}" };

                context.Variables.AddRange(x1, x2, y1, y2, y3);
                context.SaveChanges();*/

                /*InputOutput output1 = new InputOutput { Type = IOType.Output, Name = "Wynik1", TSSystemId = 1 };
                InputOutput output2 = new InputOutput { Type = IOType.Output, Name = "Wynik2", TSSystemId = 1 };

                context.InputsOutputs.Add(output1);
                context.InputsOutputs.Add(output2);
                context.SaveChanges();*/
            }
        }
    }
}
