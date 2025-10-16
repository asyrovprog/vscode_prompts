# Agent Instructions

## Common subroutines

These are subroutines that can be called from any prompt.


### EXECUTE_HALT() instructions

1: Respond to the user that you CANNOT proceed with execution of further instructions from current prompt.
2: Stop execution of current prompt and wait for the user response.

### EXECUTE_PROMPT($PROMPT) instructions

1: Reload instructions from prompt file $PROMPT. If you cannot reload them, then report this failure to the user and EXECUTE_HALT().
2: IMPORTANT: Respond to the user that you have read $PROMPT and now will now follow instructions from $PROMPT.
3: Execute instructions from prompt $PROMPT.