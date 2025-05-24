using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace Chatbot.Client.Autocomplete;

public class MediaTypeAutocompleteHandler : AutocompleteHandler
{
  private static readonly string[] types = ["image", "video", "audio"];

  public override Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
  {
    var results = new List<AutocompleteResult>();
    foreach (var type in types)
    {
      results.Add(new AutocompleteResult(type, type));
    }
    return Task.FromResult(AutocompletionResult.FromSuccess(results));
  }
}