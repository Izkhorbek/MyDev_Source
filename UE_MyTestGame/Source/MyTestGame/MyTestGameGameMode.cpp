// Copyright Epic Games, Inc. All Rights Reserved.

#include "MyTestGameGameMode.h"
#include "MyTestGameCharacter.h"
#include "UObject/ConstructorHelpers.h"

AMyTestGameGameMode::AMyTestGameGameMode()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/ThirdPersonCPP/Blueprints/ThirdPersonCharacter"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}
}
