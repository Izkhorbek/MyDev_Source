// Copyright Epic Games, Inc. All Rights Reserved.


#include "MyTestProjectGameModeBase.h"
#include "Blueprint/UserWidget.h"

void AMyTestProjectGameModeBase::BeginPlay()
{
	Super::BeginPlay();

	CurrentWidget = CreateWidget<UUserWidget>(GetWorld(), HUDWidget);
	CurrentWidget->AddToViewport();

}
