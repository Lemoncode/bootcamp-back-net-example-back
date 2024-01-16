using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lemoncode.LibraryExample.Api.Extensions
{
	internal static class ValidationExtensions
	{
		internal static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
		{
			foreach (var error in result.Errors)
			{
				modelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
		}
	}
}