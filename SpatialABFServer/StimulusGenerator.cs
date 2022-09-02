using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class StimulusGenerator
    {
        List<int>[] angles = new List<int>[4];

        public StimulusGenerator()
        {
            for (int quadrant = 0; quadrant < 4; quadrant++)
            {
                angles[quadrant] = new List<int>();

                for (int angleInterval = 0; angleInterval < 6; angleInterval++)
                {
                    angles[quadrant].Add(8 + 90 * quadrant + angleInterval * 15);
                }
            }
        }

        public Queue<int> GenerateTestAngles(int numPerQuadrant)
        {
            List<int> testAngles = new List<int>();


            foreach (List<int> quadrant in angles)
            {
                var randomizedList = RandomizeList(quadrant).ToList();
                for (int i = 0; i < Math.Min(6, numPerQuadrant); i++)
                {
                    testAngles.Add(randomizedList[i]);
                }
            }

            return new Queue<int>(RandomizeList(testAngles));
        }

        private IEnumerable<int> RandomizeList(IEnumerable<int> list)
        {
            Random rnd = new Random();
            List<int> randomisedList = list.OrderBy(i => rnd.Next()).ToList();
            return randomisedList;
        }



    }
}
