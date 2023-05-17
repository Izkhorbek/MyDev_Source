// Fill out your copyright notice in the Description page of Project Settings.


#include "MyTestAnimNotifyState.h"
#include "Engine.h"

void UMyTestAnimNotifyState::NotifyBegin(USkeletalMeshComponent* MeshComp, UAnimSequenceBase* Animation, float TotalDuration)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Magenta, __FUNCTION__);
}

void UMyTestAnimNotifyState::NotifyEnd(USkeletalMeshComponent* MeshComp, UAnimSequenceBase* Animation)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Magenta, __FUNCTION__);
}
