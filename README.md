# Generex

This library allows for the creation of regex-like patterns,
which can be used to find sequences of values in an input sequence that fulfill specific conditions.

It can of course be used to match sequences of characters - aka strings - as any regex.
However, it could also be used to evaluate and even do immediate parsing of a sequence of strings to handle paths for example.


## Optimization Concerns

To optimize the performance of finding matches in a sequence, the most important criterium is to minimize backtracking.
Backtracking happens whenever the matching process has to move back to a previous state, change it, and then try matching forward again.
The most common reason for this to happen are quantifiers, which can match less or more than they're intended to,
forcing the matching process to return to them before eventually matching the right amount.  
The most efficient patterns are thus *deterministic** ones.
To be deterministic, there can't be any uncertainty as to which sub-pattern a value in the input sequence has to be matched to at any point.

To match a sequence consisting only of the characters `a` and `b`, in which no `b` follows a `b`,
one could use the pattern: `b?(a|ab)*`. This ensures a `b` can only follow an `a` after the first letter, so there can't be any `bb`s.  
However for each `a` in the sequence, it's unclear whether it should be matched to the first or second option.
Specifically, it requires backtracking whenever a `b` appears (since the first option will be tried first).

While this might not seem like a big problem in this small example, it can become much worse with
more complex sub-patterns and even little slowdowns can add up over enough time.  
In this case, the problem could be avoided as well, by instead specifying the pattern as: `b?(a+b)*`.  
This way, each `b` after the first character must be preceeded by `a`s - no `b`s.

Non-determinism can't always be avoided, as only a subset of regular expressions is expressible deterministically.  
However it still makes sense to consider where patterns can be made as deterministic as possible.

Matches get evaluated lazily, so as long as the matches themselves are always finite, infinite input sequences pose no problem.
However this also applies to sub-matches, which means that unlimited `GreedyQuantifier<T>`s should only be used,
when it's known that there will never be an infinite sequence of matches for it.  
This is especially important for the `Wildcard<T>` pattern, which should only be used with `LazyQuantifier<T>`s
or a hard cap for the maximum number of matched input values.