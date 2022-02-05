$Stats::TextColor["White"] = "<F0>";
$Stats::TextColor["Yellow"] = "<F2>";
$Stats::TextColor["Green"] = "<F1>";
$Stats::TextColor["Red"] = "<F4>";

$Stats::ActivityCount = -1;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Rating"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Kills"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Deaths"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Cap"] = 125; // from 150
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Grab"] = 10;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Assist"] = 125;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Pickup"] = 10;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Drop"] = -5; // from -5
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Return"] = 10;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Disc Launcher"] = 20; // from 25
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Chaingun"] = 20; // from 25
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Explosives"] = 20; // from 25
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Unknown"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Teamkill"] = -5;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "MineTeamKill"] = -5;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Suicide"] = -2;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Vehicle"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Turret"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Plasma"] = 15;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Laser Rifle"] = 20;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Mortar"] = 15;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Blaster"] = 25;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Elf Gun"] = 5;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Crushed"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Explosion"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Missile"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "CarrierKill"] = 35; // from 45
//$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "StandoffReturn"] = 10; // from 25
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "MortarDeath"] = -1;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "OtherDeath"] = -2;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Disc LauncherDeath"] = -2;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "ChaingunDeath"] = -2;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "ExplosivesDeath"] = -2;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "MidAir"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "MidAirCatch"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "FlagTime"] = 1;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "GrabSpeed"] = 0;
$Stats::Rating[$Stats::Activity[$Stats::ActivityCount++] = "Damage"] = 10; // from 0
$Stats::ActivityCount++;

$Stats::TabCount = -1;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Name"] = 20;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Team"] = 10;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Rating"] = 15;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "K / D / S"] = 20;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "D / C / G"] = 20;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "CKs /-Returns"] = 15;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Caps /-Assists"] = 15;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Grabs /-Pickups"] = 15;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "MAs"] = 10;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Flag"] = 10;
$Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Damage"] = 15;
// if ($Stats::Awards::enabled)
// {
//   $Stats::Tabsize[$Stats::Tab[$Stats::TabCount++] = "Awards"] = 25;
// }
$Stats::TabCount++;

$Stats::AwardCount = -1;
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "allstar"] = 27; // Caps * CK's
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "2good"] = 1; // 15 CK's and 10 MA's
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "dutch mode"] = 20; // CKs
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "killbot"] = 35;  // kills
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "pointman"] = 5; // Caps + Assists
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "pacifist"] = 200;  // Rating / kills
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "rocket disc"] = 10;  // Midairs
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "cgpussy"] = 20; // CG kills
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "disklessonz"] = 15; // Disc kills
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "towelhead"] = 15; // Explosion kills
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "ballhog"] = 20; // Grabs + Pickups
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "rabbit"] = 120; // Flag time
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "route czar"] = 45; // Grab speed
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "pickup artist"] = 10; // most pickups
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "grabby"] = 10; // most grabs
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "rape victim"] = 20;  // Deaths
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "emokid"] = 15; // Suicides
$Stats::AwardThreshold[$Stats::Award[$Stats::AwardCount++] = "colorblind"] = 10;  // TKs
$Stats::AwardCount++;