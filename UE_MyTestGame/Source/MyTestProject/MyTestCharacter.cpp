// Fill out your copyright notice in the Description page of Project Settings.


#include "MyTestCharacter.h"
#include "GameFramework/SpringArmComponent.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/Pawn.h"
#include "GameFramework/PlayerController.h"
#include "GameFramework/Character.h"
#include <Runtime/Engine/Classes/GameFramework/CharacterMovementComponent.h>

AMyTestCharacter::AMyTestCharacter()
{
	CameraBoom = CreateDefaultSubobject<USpringArmComponent>(TEXT("CameraBoom"));
	CameraBoom->SetupAttachment(RootComponent);
	CameraBoom->TargetArmLength = 800.0f;
	CameraBoom->bUsePawnControlRotation = true;

	FollowCamera = CreateDefaultSubobject<UCameraComponent>(TEXT("FollowCamera"));
	FollowCamera->SetupAttachment(CameraBoom, USpringArmComponent::SocketName);
	FollowCamera->bUsePawnControlRotation = false;

    GetCharacterMovement()->bOrientRotationToMovement = true;
}

void AMyTestCharacter::PostInitializeComponents()
{
	Super::PostInitializeComponents();
}

void AMyTestCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	check(PlayerInputComponent);
	PlayerInputComponent->BindAxis("MoveForward", this, &AMyTestCharacter::MoveForward);
	PlayerInputComponent->BindAxis("MoveRight", this, &AMyTestCharacter::MoveRight);

	PlayerInputComponent->BindAction("Attack", IE_Released, this, &AMyTestCharacter::AttackMelee);


}

void AMyTestCharacter::MoveForward(float value)
{
	if ((Controller != NULL) && (value != 0.0f) && (isDuringAttack != true))
	{
		const FRotator Rot = Controller->GetControlRotation();
		const FRotator YawRot(0, Rot.Yaw, 0);
		const FVector  Direction = FRotationMatrix(YawRot).GetUnitAxis(EAxis::X);
		AddMovementInput(Direction, value);
		ComboAttack_Num = 0;
	}
} 

void AMyTestCharacter::MoveRight(float value)
{
	if ((Controller != NULL) && (value != 0.0f) && (isDuringAttack != true))
	{
		const FRotator Rot = Controller->GetControlRotation();
		const FRotator YawRot(0, Rot.Yaw, 0);
		const FVector Direction = FRotationMatrix(YawRot).GetUnitAxis(EAxis::Y);
		AddMovementInput(Direction, value);
		ComboAttack_Num = 0;
	}
}

