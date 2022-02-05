Attachment::AddAfter("Player::onDamage", "Midair::NadeJump::AfterOnDamage");
// Called post so we don't announce on suicide nade jumps

function Midair::NadeJump::AfterOnDamage(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
	//damage from impact etc.
	if(%type == 0)
		return;

	%time = getSimTime();

	%shooterClient = %object;
	%damagedClient = Player::getClient(%this);

	// Team damage
	if (%type == $ExplosionDamageType) {	// Self-damage, check for nade jump
		if (%shooterClient == %damagedClient) {
		 	if (%time == %shooterClient.lastNadeCollisionTime) {
				Midair::AnnounceNadeJump(%shooterClient);
			}
		}
	}
}

function Midair::AnnounceNadeJump(%cl)
{
	// Called in RocketDumb::onCollision()
	Client::SendMessage(%cl, 0, "~wmine_act.wav");
	%vel = Item::GetVelocity(Client::GetControlObject(%cl));
	%speed = Vector::GetDistance("0 0 0", %vel);
	MessageAll(0, Client::GetName(%cl) @ " hit a nade jump going [ " @
		floor(%speed) @ " meters / second ] !");
}

