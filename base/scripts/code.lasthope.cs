//
// rewritten code.lasthope.cs v2013 by opsayo
// this is meant for use with 1.41 clients
// unfortunately there are no random memory scans
// so this won't catch new cheats
//
// based on the v2009.46 version by bugs_
// which was based on zadmin93 and 93++ by Andrew.
//
// does anyone have the version that used to have the 1.30 happymod addresses?
//

function Lasthope::AttachToEvents()
{
  Attachment::AddAfter("Server::loadMission",  "Lasthope::resetClients");
  Attachment::AddAfter("Game::startMatch",  "Lasthope::schedulePeriodicCheck");
  Attachment::AddAfter("Game::startHalf",  "Lasthope::schedulePeriodicCheck");
  Attachment::AddAfter("Server::onClientConnect",  "Lasthope::scheduleInitClient");
}

function Lasthope::resetClients(%missionname, %immed)
{
  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
  {
    LastHope::ResetClient(%cl);
  } 
}

function Lasthope::initClients()
{
  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
  {
    //LastHope::InitClient(%cl);
    LastHope::InitClient(%cl, Client::GetTransportAddress(%cl));
  } 
}

function Lasthope::schedulePeriodicCheck()
{
  schedule("Lasthope::PeriodicCheck();", 10);
}

function Lasthope::scheduleInitClient(%clientId)
{
  schedule("LastHope::InitClient("@%clientId@",\""@Client::GetTransportAddress(%clientId)@"\");", 30);
}

function Lasthope::InitServer()
{
  //$k = "";
  //remoteLH_CHMEM(2048,0,0,0,q);
  //if ($k == "")
  //{
    //echo("Error: You need the 1.30 LastHope Server .exe (www.lasthope.us)");
    //return "False";
  //}
  Lasthope::AttachToEvents();
}

if ($Lasthope::enabled)
  Lasthope::InitServer();

function Lasthope::EnableLasthope(%cl)
{
  $Lasthope::enabled = true;
  Lasthope::initServer();
  messageAll(1, Client::getName(%cl) @ " just enabled lasthope! It can only be disabled by a server restart.~wmine_act.wav");
  Lasthope::initClients();
}

function Lasthope::DisableLasthope(%cl)
{
  $Lasthope::enabled = false;
  messageAll(1, Client::getName(%cl) @ " just disabled lasthope! It will take a few minutes to come to a stop.~wLeftMissionArea.wav");
}


$cheatLogFile = "config\\cheat.log";
$kickLogFile = "config\\kick.log";

$LH_Debug = true;
$LH_RetryLimit = 4;
$LH_Response = 5;

// query types
$LH_Model = 0;
$LH_Memory = 1;
$LH_141_Version = 2;
$LH_140_Version = 3;

// actions
$LH_Kick = 0;
$LH_Log = 1;
$LH_DoNothing = 2;

$LH_Period_Time = 120;
// Used in stats screen, toggle in dmt.config.cs
// $LastHope::strict = false;
$i = -1;

// Always check client version first
// The version logic works as follows:
// - Look for a file that no clients have, and look for the unique 1.41 NULL value
// - Look for a while that 1.30 should not have, and look for 1.30 NULL value
// - All other clients should be 1.40.
// We have a fallback solution which is more complicated but we don't use:
// The 1.41 client returns different values from 1.30 and 1.40. So we query a file that we know will return null.
// The 1.30 client returns null the first time you ask for a file, and the real value the second time.
// The 1.40 client returns the real value both times.
// Client null values:
// 1.30 and 1.40: 730046966
// 1.41: -2070136960

// XXX Be careful if you change the order of these! Some of the logic expects this ordering.
// $LH_Type[$i++] = $LH_141_Version; $LH_Target[$i] = "afterhope_config_file.cs"; // Any file that doesn't exist really
// $LH_Msg[$i] = "client version"; $LH_GoodValue[$i] = -1; $LH_Action[$i] = $LH_DoNothing;
$LH_Type[$i++] = $LH_141_Version; $LH_Target[$i] = "DangerousCrossing.dsc"; // 1.41 is the only client which returns non-null the first time
$LH_Msg[$i] = "client version"; $LH_GoodValue[$i] = -1; $LH_Action[$i] = $LH_Skip;
$LH_Type[$i++] = $LH_140_Version; $LH_Target[$i] = "DangerousCrossing.dsc"; // 1.40 is the only client which returns null the second time
$LH_Msg[$i] = "client version"; $LH_GoodValue[$i] = -1; $LH_Action[$i] = $LH_DoNothing;

// Then cheat tests. Theoretically these should always be LH_Kick.

// lflame check
$LH_Type[$i++] = $LH_Model;
$LH_Target[$i] = "lflame.dts";
$LH_Msg[$i] = "lflame";
$LH_GoodValue[$i] = -1196499584;
$LH_Action[$i] = $LH_Log;

// minimapterrain from happymod
$LH_Type[$i++] = $LH_Memory;
$LH_Target[$i] = 0x00496b9b;
$LH_Msg[$i] = "bad memory";
$LH_GoodValue[$i] = 616822535;
$LH_Action[$i] = $LH_Kick;

// flag check.
$LH_Type[$i++] = $LH_Model;
$LH_Target[$i] = "flag.dts";
$LH_Msg[$i] = "flag";
$LH_GoodValue[$i] = 899608146;
$LH_Action[$i] = $LH_Kick;

// jet pack check
$LH_Type[$i++] = $LH_Model;
$LH_Target[$i] = "jetpack.dts";
$LH_Msg[$i] = "jetpack";
$LH_GoodValue[$i] = 1662548082;
$LH_Action[$i] = $LH_Kick;

// Then queries for fun
//$LH_Type[$i++] = $LH_Model; $LH_Target[$i] = "bugs";
//$LH_Msg[$i] = "bugs_bypass"; $LH_GoodValue[$i] = -2070136960; $LH_Action[$i] = $LH_DoNothing;

// random address 1
$LH_Type[$i++] = $LH_Memory ; $LH_Target[$i] = 0x00401313;
$LH_Msg[$i] = "random_addr1"; $LH_GoodValue[$i] = 136959862; $LH_Action[$i] = $LH_Kick;

// random address 2
// This will crash non 1.41 clients (I think)
// New Random Check - will kick for unresponsive client, so we need to watch this(I think)
$LH_Type[$i++] = $LH_Memory;
$LH_Target[$i] = 0x0054175C;
$LH_Msg[$i] = "random_addr2";
$LH_GoodValue[$i] = 987287641;
$LH_Action[$i] = $LH_DoNothing;

//$LH_Msg[$i] = "random_addr1"; $LH_GoodValue[$i] = 973654280; $LH_Action[$i] = $LH_Log;
//$LH_Type[$i++] = $LH_Memory ; $LH_Target[$i] = 0x00497253;  // minimapiff
//$LH_Msg[$i] = "happymod2"; $LH_GoodValue[$i] = 388240885 $LH_Action[$i] = $LH_Kick;;
//$LH_Type[$i++] = $LH_Memory ; $LH_Target[$i] = 0x0040138a;
//$LH_Msg[$i] = "random_mem2"; $LH_GoodValue[$i] = 476377398 $LH_Action[$i] = $LH_Kick;;



$LH_Max_Test = $i;

function dbecho(%level, %str){}

function remoteK(%cl, %result)
{
  %ip = Client::GetTransportAddress(%cl);
  if ((%ip == "") || (%ip != %cl.LHIP)) return;
  if ($LH_Debug) echo("remoteK "@ %cl @" "@ %result);
  if (%cl.LHinit == true) %cl.LHResult = %result;
}

function LH::delay(%mode)
{
  %random_delay = floor(getRandom() * 7) + 8 + (32 * %mode);
  return(%random_delay);
}

function LH::Kick(%cl, %msg)
{
  if ((%cl <= 0) || (%cl.gone)) return;
  %cl.gone = true;
  %name = Client::getName(%cl);
  %kickMsg = "LH141++ " @ %name @ " " @ %msg @ " kick";
  messageall(0, %kickMsg);
  echo(%kickMsg);
  //messageall(0,"LH141++ "@ %name @" "@ %msg @" kick");
  //echo("LH141++ "@ %name @" "@ %msg @" kick");
  schedule("Net::Kick(" @ %cl @ ",'Come back without the " @ %msg @ "');",1);

  %date = zadmin::getTimeStamp_Patched();
  %name = Client::getName(%client);
  %ip = Client::getTransportAddress(%client);
  $kickLogEntry = %date @ "Name: " @ %name @ ", IP: " @ %ip @ ", Msg: " @ %kickMsg;

  echo($kickLogFile);
  export("$kickLogEntry", $kickLogFile, true);
}

function LH::VersionKick(%cl,%msg)
{
  if ((%cl <= 0) || (%cl.gone)) return;
  %cl.gone = true;
  %name = Client::getName(%cl);
  messageall(0,"LH141++ "@ %name @" "@ %msg @" kick");
  echo("LH141++ "@ %name @" "@ %msg @" kick");
  schedule("Net::Kick(" @ %cl @ ",'This server requires Tribes 1.41. Visit tinyurl.com/tribes141 to get it.');",1);
}

function LH::Gone(%cl,%tag)
{
  if (%cl <= 0) return true;
  %ip = Client::GetTransportAddress(%cl);
  if ((%ip == "") || (%ip != %cl.LHIP) || (%tag != %cl.LHTag)) return true;
  if (%cl.LHinit != true) return true;
  return false;
}

function LH::CheckClient(%cl,%tag)
{
  if (LH::Gone(%cl,%tag) == true) return;

  // Retry logic
  if (%cl.LHResult == "")
  {
    echo("LHRetries:"@ Client::GetName(%cl) @ " - " @ %cl.LHTest @ " - " @ %cl.LHRetries @ " - " @ %cl.LHResult);
    if (%cl.LHRetries++ >= $LH_RetryLimit)
    {
      if (%cl.client == "1.41")
      {
        if ($LH_Target[%cl.LHTest] == "bugs")
        {
          %msg = "Stopped responding: " @ $LH_Target[%cl.LHTest];
          echo (%msg);
          LH::logCheat(%cl, %msg);
        }
        else
          LH::Kick(%cl,"unresponsive AfterHope client");
      }
      else if ($LH_Type[%cl.LHTest] == $LH_141_Version)
      {
        %cl.client = "1.11";
        echo("LH::CheckClient max retries, assuming 1.11");
        if ($Lasthope::strict)
        {
          echo("VersionKick: "@ Client::GetName(%cl) @", Test: "@ %cl.LHTest @" - "@ $LH_Msg[%cl.LHTest] @", Result: "@ %cl.LHResult);
          LH::VersionKick(%cl,$LH_Msg[%cl.LHtest]);
        }
      }
    }
    else
      schedule("LH::TestClient(" @ %cl @ "," @ %tag @ ");",$LH_Response);

    return;
  }

  %cl.LHtimestamp = getIntegerTime(true);

  if ($LH_Debug)
    echo("LH::CheckClient " @ Client::GetName(%cl) @ ", Test: " @ %cl.LHTest @ " - " @ $LH_Msg[%cl.LHTest] @ ", Result: " @ %cl.LHResult @ ", Expected: " @ $LH_GoodValue[%cl.LHTest]);

  if ($LH_Type[%cl.LHTest] == $LH_141_Version)
  {
    // The version check only can be done once per connection
    if (%cl.client)
    {
      if (%cl.client == "1.41")
        schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    }
    else
    {
      // 1.41 is the only client which doesn't return null the first time
      if (%cl.LHResult != 730046966)
      {
        echo("LH::CheckClient " @ Client::GetName(%cl) @ " set to 1.41. Test: " @ %cl.LHTest @ ", Response: " @ %cl.LHResult);
        %cl.client =  "1.41";
      }
      schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    }
  }
  else if ($LH_Type[%cl.LHTest] == $LH_140_Version)
  {
    // The version check only can be done once per connection
    // if (%cl.client)
    // {
    //   if (%cl.client == "1.41")
    //     schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    // } 
    // else
    if (!%cl.client)
    {
      // 1.40 is the only client which returns null the second time
      if (%cl.LHResult == 730046966)
      {
        %cl.client = "1.40";
      }
      else if (%cl.client != "1.41")
      {
        // If the client isn't 1.41 already, then it must be 1.30.
        %cl.client = "1.30";
      }
      echo("LH::CheckClient " @ Client::GetName(%cl) @ " set to " @ %cl.client @". Test: " @ %cl.LHTest @ ", Response: " @ %cl.LHResult);
    }

    // This is the last version test, so check the client version now and whether we should continue or kick
    if (%cl.client == "1.41")
    {
      // Continue testing if they're running 1.41.
      schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    }
    else if ($Lasthope::strict)
    {
      // Kick if strict mode
      echo("VersionKick: "@ Client::GetName(%cl) @", Test: "@ %cl.LHTest @" - "@ $LH_Msg[%cl.LHTest] @", Result: "@ %cl.LHResult);
      LH::VersionKick(%cl,$LH_Msg[%cl.LHtest]);
    }
  }

  // // The version logic works as follows:
  // // The 1.41 client returns different values from 1.30 and 1.40. So we query a file that we know will return null.
  // // The 1.30 client returns null the first time you ask for a file, and the real value the second time.
  // // The 1.40 client returns the real value both times.
  // if (false && $LH_Type[%cl.LHTest] == $LH_141_Version)
  // {

  //   // Store the version responses so we can compare consecutive ones.
  //   $LH_Version_Responses[%cl.LHTest] = %cl.LHResult;

  //   // The first test we just query something that doesn't exist, and check the null value.
  //   if (%cl.LHTest == 0)
  //   {
  //     if (%cl.LHResult == -2070136960) // 1.41 null. We have to hardcode it here because Tribes will round the number.
  //     {
  //       %cl.client = "1.41";
  //       echo("LH::CheckClient " @ Client::GetName(%cl) @ " set to 1.41");
  //     }
  //     else if (%cl.LHResult != 730046966) // 1.30, 1.40 null. 
  //     {
  //       // This is suspicious. The first test should definitely return some kind of null.
  //       echo("LH::CheckClient checking version but non-existent file returned non-null: " @ %cl.LHResult);
  //     }
  //   }

  //   // XXX Hardcoded test indices here!
  //   if (%cl.client != "1.41" && (%cl.LHTest == 2 || %cl.LHTest == 4))
  //   {
  //     $first_idx = %cl.LHTest - 1;
  //     $second_idx = %cl.LHTest;
  //     echo("LH::CheckClient (" @ %cl.LHTest @ ") " @ $LH_Target[%cl.LHTest] @ " responses: " @ $LH_Version_Responses[$first_idx] @ ", " @ $LH_Version_Responses[$second_idx]);
  //     if ($LH_Version_Responses[2] == 730046966)
  //     {
  //       // The second query should be non-null. Since it is null, the client doesn't have the file we're trying to test.
  //       // This is a little weird. so we log something. Log and try the next file.
  //       echo("LH::CheckClient checking version but second query was null");
  //     }
  //     else if ($LH_Version_Responses[1] == $LH_Version_Responses[2])
  //     {
  //       // Two of the same non-null responses should be 1.40.
  //       echo("LH::CheckClient " @ Client::GetName(%cl) @ " set to 1.40");
  //       %cl.client = "1.40";
  //     }
  //     else
  //     {
  //       if ($LH_Version_Responses[1] == 730046966)
  //       {
  //         // 1.30 returns two different responses, first one should be null
  //         echo("LH::CheckClient " @ Client::GetName(%cl) @ " set to 1.30");
  //         %cl.client = "1.30";
  //       }
  //       else
  //       {
  //         // Something weird here, different non-null responses is unexpected. Log and try the next file.
  //         echo("LH::CheckClient checking version but both queries were non-null");
  //       }
  //     }
  //   }

  //   // The last version test, so check results and proceed accordingly
  //   if (%cl.LHTest == 4)
  //   {
  //     if (%cl.client == "1.41")
  //     {
  //       // Only continue testing if they're running 1.41.
  //       schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
  //     }
  //     else if ($Lasthope::strict)
  //     {
  //       // Kick if strict mode
  //       echo("VersionKick: "@ Client::GetName(%cl) @", Test: "@ %cl.LHTest @" - "@ $LH_Msg[%cl.LHTest] @", Result: "@ %cl.LHResult);
  //       LH::VersionKick(%cl,$LH_Msg[%cl.LHtest]);
  //     }
  //   }
  //   else
  //   {
  //     schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode)); 
  //   }

    // Old version code, afraid to delete it (8/17/2018). If enough time passes then delete it.
    // echo("LHCS:"@ Client::GetName(%cl) @ " - " @ %cl.LHcs @ " - " @ %cl.LHResult);
    // if (%cl.LHcs++ == 1)
    // {
    //   schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    // }
    // else if (%cl.LHcs >= 2)
    // {
    //   schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    //   %cl.LHcs = 0;
    //   if (%cl.LHResult == -2070136960)
    //   {
    //     %cl.client = "1.41";
    //     // schedule("LH::SelectTest( "@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    //   }
    //   else if (%cl.LHResult == 730046966) // 1.40
    //   {
    //     if ($Lasthope::strict)
    //     {
    //       echo("VersionKick: "@ Client::GetName(%cl) @", Test: "@ %cl.LHTest @" - "@ $LH_Msg[%cl.LHTest] @", Result: "@ %cl.LHResult);
    //       LH::VersionKick(%cl,$LH_Msg[%cl.LHtest]);
    //     }
    //     else
    //       %cl.client = "1.40";
    //   }
    //   else
    //   {
    //     if ($LastHope::strict)
    //       LH::VersionKick(%cl,$LH_Msg[%cl.LHtest]);
    //     else
    //       %cl.client = "1.30";
    //   }
  else if ($LH_Type[%cl.LHtest] == $LH_Model || $LH_Type[%cl.LHtest] == $LH_Memory)
  {
    if (%cl.LHResult != $LH_GoodValue[%cl.LHtest])
    {
      if ($LH_Action[%cl.LHtest] == $LH_Kick)
      {
        echo("Kick: "@ Client::GetName(%cl) @", Test: "@ %cl.LHTest @" - "@ $LH_Msg[%cl.LHTest] @", Result: "@ %cl.LHResult);
        LH::Kick(%cl,$LH_Msg[%cl.LHtest]);
      }
      else if ($LH_Action[%cl.LHtest] == $LH_Log)
      {
        if (%cl.LHResult != -1196499584) {
          %msg = "Bad result: " @ $LH_Target[%cl.LHTest] @ ", Test: " @ %cl.LHTest @ " - " @ $LH_Msg[%cl.LHTest] @ ", Result: " @ %cl.LHResult;
          echo (%msg);
          LH::logCheat(%cl, %msg);
        }
        schedule("LH::SelectTest("@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode)); // Moved down so it continues the check. Was stopping at lflame
      }
      else if ($LH_Action[%cl.LHtest] == $LH_DoNothing)
      {
        %msg = "DoNothing, bad value: " @ $LH_Target[%cl.LHTest] @ ", Test: " @ %cl.LHTest @ " - " @ $LH_Msg[%cl.LHTest] @ ", Result: " @ %cl.LHResult;
        echo (%msg);
        schedule("LH::SelectTest("@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
      }
      else
      {
        echo ("Unknown action: " @ $LH_Action[%cl.LHTest] @ ", Target: " @ $LH_Target[%cl.LHtest] @ ", Test: " @ %cl.LHTest @ " - " @ $LH_Msg[%cl.LHTest]);
      }
    }
    else
    {
      schedule("LH::SelectTest("@%cl@","@%cl.LHTag@");", LH::delay(%cl.LHmode));
    }
  }
}

function LH::SelectTest(%cl,%tag)
{
  if (LH::Gone(%cl,%tag) == true) return;
  if (%cl.LHmode == 0)
  {
    %cl.LHtest++;
    if (%cl.LHtest > $LH_Max_test)
      %cl.LHmode = 1;
  }

  while ((%cl.LHmode == 1) && ($LH_Action[%cl.LHtest] != $LH_Kick))
    %cl.LHtest = LH::GetRandomTest();

  %cl.LHRetries = 0;
  if ($LH_Debug)
    echo("LH::SelectTest "@ %cl @" test is "@ %cl.LHtest @" "@ $LH_Target[%cl.LHtest]);

  schedule("LH::TestClient("@%cl@","@%tag@");", LH::delay(%cl.LHmode));
}

function LH::GetRandomTest()
{
  %test = floor(getRandom() * $LH_Max_Test) + 1;
  return %test;
}

function LH::TestClient( %cl, %tag )
{
  if (LH::Gone(%cl,%tag) == true) return;
  %cl.LHResult = "";
  %target = $LH_Target[%cl.LHTest];
  if ($LH_Type[%cl.LHtest] == $LH_Model)
    remoteEval(%cl,LH_CHMOD,bugs,bugs,%target);
  else if ($LH_Type[%cl.LHtest] == $LH_Memory)
    remoteEval(%cl,LH_CHMEM,%target,%target,%target,bugs);
  else if ($LH_Type[%cl.LHtest] == $LH_141_Version)
    remoteEval(%cl,LH_CHMOD,bugs,bugs,%target);
  else if ($LH_Type[%cl.LHtest] == $LH_140_Version)
    remoteEval(%cl,LH_CHMOD,bugs,bugs,%target);

  schedule("LH::CheckClient(" @ %cl @ "," @ %tag @ ");", $LH_Response);
}

function LastHope::InitClient(%cl, %clIP)
{
  if (%cl <= 0) return;
  %ip = Client::GetTransportAddress(%cl);
  if (%ip == "") return;

  if (%cl.LHInit == true && %cl.LHip == %ip)
  {
    %now = getIntegerTime(true);
    %delta = %now - %cl.LHtimestamp;
    if ((%delta >= 0) && (%delta < 1920)) return;
  }

  %cl.LHInit = true;
  %cl.LHTag = floor(getRandom() * 32767) + 1;
  %cl.LHtimestamp = getIntegerTime(true);
  %cl.LHIP = %ip;
  %cl.LHmode = 0; // 0 sequential, 1 random
  %cl.LHtest = -1; // +1 = first test;
  %cl.LHcs = 0;
  %random_delay = floor(getRandom() * 7) + 8;
  schedule("LH::SelectTest(" @ %cl @ "," @ %cl.LHTag @ ");", %random_delay);
  if ($LH_DEBUG)
    echo ("Client initialized: (" @ %cl @ ", " @ %cl.LHTag @ ")");
}

function LastHope::ResetClient(%cl)
{
  if ($LH_Debug) echo("LastHope::ResetClient(" @ %cl @ ")");
  %cl.LHInit = "";
  %cl.LHTag = 0;
}

function LastHope::PeriodicCheck()
{
  if ($LH_Debug) echo("LastHope::PeriodicCheck()");
  for (%cl=Client::getFirst(); %cl != -1; %cl=Client::getNext(%cl))
    LastHope::InitClient(%cl, Client::GetTransportAddress(%cl));
  // schedule("LastHope::PeriodicCheck();", $LH_Period_Time);
}

function LH::MessageAll(%msg)
{
  MessageAll(0, %msg);
}

// stolen from code.iplog.cs
function LH::logCheat(%client, %msg)
{
  if( %client != "" && %client > 0 ) // prevent fuckupsssssss
  {
    %date = zadmin::getTimeStamp_Patched();
    %name = Client::getName(%client);
    %ip = Client::getTransportAddress(%client);

    $cheatLogEntry = %date @ "Name: " @ %name @ ", IP: " @ %ip @ ", Msg: " @ %msg;

    echo($cheatlogEntry);
    export("$cheatLogEntry", $cheatLogFile, true); // append the entries
  }
}

