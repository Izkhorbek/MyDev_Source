// Fill out your copyright notice in the Description page of Project Settings.

#include "BasicCharacter.h"
#include "Engine.h"
#include "Kismet/GameplayStatics.h"
#include "MyTestWeaponActor.h"
#include "Animation/AnimMontage.h"
#include "Animation/AnimInstance.h"
#include "Containers/Array.h"
#include "Engine/World.h"

// Sets default values
ABasicCharacter::ABasicCharacter()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	isDuringAttack = false;
	ComboAttack_Num = 0;
}

/*****************Weapon*********************/
USkeletalMeshComponent* ABasicCharacter::GetSpecificPawnMesh() const
{
	return GetMesh();
}

FName ABasicCharacter::GetWeaponAttachPoint() const
{
	return WeaponAttachPoint;
}

void ABasicCharacter::EquipWeapon(AMyTestWeaponActor* Weapon)
{
	if (Weapon)
	{
		SetCurrentWeapon(Weapon, CurrentWeapon);
	}
}


void ABasicCharacter::AddWeapon(AMyTestWeaponActor* Weapon)
{
	if (Weapon)
	{
		Inventory.AddUnique(Weapon);
	}
}

void ABasicCharacter::SetCurrentWeapon(AMyTestWeaponActor* NewWeapon, AMyTestWeaponActor* LastWeapon)
{
	AMyTestWeaponActor* LocalLastWeapon = NULL;

	if (LastWeapon != NULL)
	{
		LocalLastWeapon = LastWeapon;
	}

	if (NewWeapon)
	{
		NewWeapon->SetOwningPawm(this);
		NewWeapon->OnEquip(LastWeapon);
	}
}
void ABasicCharacter::SpawndefaultInventory()
{
	int32 NumWeaponClasses = DefaultInventoryClasses.Num();
	if (DefaultInventoryClasses[0])
	{
		FActorSpawnParameters SpawnInfo;
		UWorld* WRLD = GetWorld();
		AMyTestWeaponActor* NewWeapon = WRLD->SpawnActor<AMyTestWeaponActor>(DefaultInventoryClasses[0], SpawnInfo);
		AddWeapon(NewWeapon);
	}

	if (Inventory.Num() > 0)
	{
		EquipWeapon(Inventory[0]);
	}
}
//****************************************
// Called every frame
void ABasicCharacter::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void ABasicCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

}

void ABasicCharacter::AttackMelee()
{
	if (ComboAttack_Num  < 3)
	{
		int32 tmp_Num = rand() % 3 + 1;
		
		FString PlaySection = "Attack_" + FString::FromInt(tmp_Num);

		PlayAnimMontage(Attack_Melee_AnimMt, 1.1f, FName(*PlaySection));
		GEngine->AddOnScreenDebugMessage(-1, 5.0f, FColor::Red, TEXT("Attack" + PlaySection));

		ComboAttack_Num++;

		isDuringAttack = true;
	}
	else
	{
		PlayAnimMontage(LastAttack_Anims, 1.1f);
		ComboAttack_Num = 0;
	}

	FTimerHandle TH_Attack_End;
	GetWorldTimerManager().SetTimer(TH_Attack_End, this, &ABasicCharacter::AttackMelee_End, 1.7f, false);
}

void ABasicCharacter::AttackMelee_End()
{
	isDuringAttack = false;
	
	StopAnimMontage(Attack_Melee_AnimMt);
	StopAnimMontage(LastAttack_Anims);

}

