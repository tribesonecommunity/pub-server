function Midair::onMidairDisc(%clOwner, %clTarget, %time)
{
	Client::AdjustScoreNoUpdate(%clOwner, "MidAir");
	Client::AdjustScoreNoUpdate(%clOwner, "MidAirDist", %time);
	Client::AdjustScoreNoUpdate(%clTarget, "MidAirCatch");

  %clOwner.lastMidairTarget = %clTarget;
  %clOwner.lastMidairTime = getSimTime();

	MessageAll(0, Client::GetName(%clOwner) @ " lands [ " @
		floor(%time * 0.065) @ " meter ] mid-air on " @ Client::GetName(%clTarget) @ "!");
	Client::SendMessage(%clOwner, 0, "~wc_buysell.wav");

	%meters = floor(%time * 0.065);
	//if (%meters > 50)
		//Announcer::announce("~wheadshot.wav");
	//zadmin::ActiveMessage::All( MidairDisc, Client::GetName(%clOwner), Client::GetName(%clTarget), %time );
}

