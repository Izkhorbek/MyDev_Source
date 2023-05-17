// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "BasicCharacter.h"
#include "Components/SkeletalMeshComponent.h"
#include "UObject/UObjectGlobals.h"
#include "MyTestWeaponActor.generated.h"

UCLASS()
class MYTESTPROJECT_API AMyTestWeaponActor : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AMyTestWeaponActor(const class FObjectInitializer& ObjectInitilizer);

	void SetOwningPawm(ABasicCharacter* NewOwner);
	void AttachMeshToPawn();
	void OnEquip(const AMyTestWeaponActor* LastWeapon);

	virtual void NotifyActorBeginOverlap(AActor* OtherActor)override;

	UPROPERTY(EditDefaultsOnly, Category = "MyFX")
		UParticleSystem* HitFX;

	// Called every frame
	virtual void Tick(float DeltaTime) override;
private:
	UPROPERTY(VisibleDefaultsOnly, Category = Weapon)
		USkeletalMeshComponent* WeaponMesh;

	UPROPERTY(VisibleDefaultsOnly, Category = Weapon)
		class UBoxComponent* WeaponCollision;

protected:
	class ABasicCharacter* MyPawn;
	virtual void BeginPlay() override;

};
