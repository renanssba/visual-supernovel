How to create a custom command:

Create a new class with a name following the pattern of the other commands: Function + "Command" sufix.
"ChangeClothesCommand", "SendHighscoreCommand", etc, are valid command names.

After that, make sure you've done the following:

1) Wrap your code around a  "namespace Command{ <your code here> }". If you don't do this, VSN won't recognize your command.

2) Extend the class "VsnCommand" (e.g "public class ExampleCommand : VsnCommand").

3) Implement the methods required from the VsnCommand.

4) Before class declaration, put the following attribute without quotation marks: "[CommandAttribute(CommandString="example")]". This will ensure that VSN knows what the command is gonna be called inside a VSN script (e.g in "char_alpha 0.3", "char_alpha" is the CharAlphaCommand attribute).

At the present time, you have to manually inject the arguments with the "InjectArguments" method.

If you need help, just copy another simple command class from Core (e.g the "GotoCommand.cs") and edit it where is needed.