Attachment::AddBefore("Player::onDamage",	"HitSounds::OnDamage");

function HitSounds::OnDamage(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
	if ($HitSounds::enabled)
	{
		//damage from impact etc.
		if(%type == 0)
			return;

		if(%object.hitSoundsDisabled || !$HitSounds::enabled)
			return;

		%shooterClient = %object;
		%damagedClient = Player::getClient(%this);
		%damagedClientTeam = Client::getTeam(%damagedClient);
		%shooterClientTeam = Client::getTeam(%shooterClient);

		// Team damage not on self
		if (%shooterClientTeam != %damagedClientTeam) {
			if (%shooterClient != %damagedClient) {
                //%box1 = getboxcenter(client::getownedobject(%shooterClient));
                //%box2 = getboxcenter(client::getownedobject(%damagedClient));
                //%mask = 1+2+4+8+32+64 ;
                //0 = NOTHING
                //1 = Terrain and Interior
                //2 = Static Shapes
                //4 = Moveables
                //8 = Vehicles
                //16 = Players
                //32 = Mines
                //64 = unknown object type - FORCE FIELD?
                //if (getlosInfo(%box1,%box2,%mask)) {
                    //Client::sendMessage(%shooterClient, 0, "~whit.wav");
                //}
                //else {
                    Client::sendMessage(%shooterClient, 0, "~whit.wav");
//                }
			}
		}
	}
}
