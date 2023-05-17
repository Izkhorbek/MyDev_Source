// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "Animation/AnimMontage.h"
#include "BasicCharacter.generated.h"


UCLASS()
class MYTESTPROJECT_API ABasicCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	ABasicCharacter();

	USkeletalMeshComponent* GetSpecificPawnMesh() const;

	FName GetWeaponAttachPoint()const;
	
	void EquipWeapon(class AMyTestWeaponActor* Weapon);

protected:

	UPROPERTY(EditDefaultsOnly, Category = Inventory)
	FName WeaponAttachPoint;

	TArray<AMyTestWeaponActor*> Inventory;
	
	class AMyTestWeaponActor* CurrentWeapon;

	UPROPERTY(EditDefaultsOnly, Category = Inventory)
		TArray<TSubclassOf<class AMyTestWeaponActor >> DefaultInventoryClasses;

	void AddWeapon(class AMyTestWeaponActor* Weapon);

	void SetCurrentWeapon(class AMyTestWeaponActor* NewWeapon, class AMyTestWeaponActor* LastWeapon);

	void SpawndefaultInventory();


public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	UFUNCTION(BlueprintCallable)
	void AttackMelee();

	UFUNCTION(BlueprintCallable)
	void AttackMelee_End();
	
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = Pawn)
		UAnimMontage* Attack_Melee_AnimMt;

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = Pawn)
		UAnimMontage* LastAttack_Anims;

	UPROPERTY(EditDefaultsOnly, Category = Pawn)
		TArray<TSubclassOf<UAnimMontage>> Attack_Melee_Anims;

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	bool isDuringAttack = false;

	int32 ComboAttack_Num;

	UPROPERTY(EditDefaultsOnly, Category = "MyFX")
		UParticleSystem* HitFX;
};
