//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the none tool that
    the beast player is born into the game with by
    default. This tool does nothing except prompt
    the player to press 2 to use the rock transformation
    tool that the beast player also has when the game
    starts.
*/

public class Client_BeastNone : ClientHumanItem
{
    protected override void Enabled()
    {
        Refresh("Press 2 to disguise yourself as a rock");
    }
}
