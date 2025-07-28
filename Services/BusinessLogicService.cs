using FDWS.Models;
using FDWS.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;

namespace FDWS.Services
{
    public class BusinessLogicService : IBusinessLogic
    {
        public async Task<object> GetHomeDataAsync()
        {
            // Simulate async operation
            await Task.Delay(100);
            
            return new
            {
                Title = "Welcome to FDWS",
                Message = "Your application is running successfully!",
                Timestamp = DateTime.Now
            };
        }

        private BigInteger GenerateFibonacci(int n) 
        {
            if (n < 0) throw new ArgumentException("Input must be a non-negative integer.");
            if (n == 0) return 0;
            if (n == 1) return 1;

            BigInteger sum = 1;
            BigInteger a = 0, b = 1;
            for (int i = 2; i <= n; i++)
            {
                BigInteger temp = a + b;
                sum += temp;
                a = b;
                b = temp;
            }

            return sum;
        }

        private double GetVillagerNumber(int currentYear) 
        {
            var getResult = GenerateFibonacci(currentYear);

            if (getResult == 0) return 0;

            double logVal = BigInteger.Log10(getResult);

            return Math.Pow(10, logVal % 10);
        }

        public async Task<ResponseObject> ValidateInputAsync(List<int[]> listData) 
        {
            ResponseObject response = new ResponseObject();

            if (listData == null || listData.Count == 0)
            {
                response.Result = "No input";
                response.Status = HttpStatusCode.BadRequest.ToString();
            }
            else
            {
                double deathAverage = 0.0;
                double tempTotalDeath = 0.0;
                
                foreach (var data in listData)
                {
                    int ageOfDeath = data[0];
                    int yearOfDeath = data[1];

                    if (yearOfDeath < ageOfDeath) 
                    {
                        response.Result = "Some of the input is invalid (Year of Death must be higher than Age of Death)";
                        response.Status = HttpStatusCode.BadRequest.ToString();
                        return response;
                    }

                    tempTotalDeath += GetVillagerNumber(yearOfDeath - ageOfDeath);
                }

                deathAverage = tempTotalDeath / listData.Count;

                response.Result = deathAverage.ToString();
                response.Status = HttpStatusCode.OK.ToString();
            }

            return response;
        }
    }
}