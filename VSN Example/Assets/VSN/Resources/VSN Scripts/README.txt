Quick infos:

1) The character command uses the following syntax:

character "label" "spritename"

It creates a new character with the nickname "Label" and with the sprite "spritename", which will be linked with the respective "spritename"
file in the "Resources/Characters" folder.

Its default position is something like x = 0.5 and y = 0.2.

2) Some commands accept different signatures.

Example: The Flash Command.

flash 0.5
This will flash with a duration of 0.5 seconds.

flash
This will flash for the default duration for the VSN (which is 0.2)

All the commands like "char_alpha", "char_movex" etc accept an additional argument: the duration.
For an example, please look at the "Resources/VSN Scripts/example3" script (and example4 as well).