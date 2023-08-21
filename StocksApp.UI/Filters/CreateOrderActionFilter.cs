using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StocksApp.Controllers;
using StocksApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StocksApp.Filters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IOrderRequest orderRequest = (IOrderRequest)context.ActionArguments["orderRequest"];
            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            if (context.Controller is TradeController tradeController)
            {
                var validationResultList = new List<ValidationResult>();
                bool modelIsValid = Validator.TryValidateObject(orderRequest, new ValidationContext(orderRequest), validationResultList, true);

                if (!modelIsValid)
                {
                    tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    StockTrade stockTrade = new StockTrade
                    {
                        StockSymbol = Convert.ToString(orderRequest.StockSymbol),
                        StockName = Convert.ToString(orderRequest.StockName),
                        Price = Convert.ToDouble((orderRequest.Price), CultureInfo.InvariantCulture)
                    };

                    context.Result = tradeController.View("Index", stockTrade);
                }
                else
                    await next();
            }
            else await next();
        }
    }
}
