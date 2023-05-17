// Fill out your copyright notice in the Description page of Project Settings.

#include "MyTestWeaponActor.h"
#include "Components/BoxComponent.h"
#include "Engine.h"
// Sets default values
AMyTestWeaponActor::AMyTestWeaponActor(const class FObjectInitializer& ObjectInitializer) : Super(ObjectInitializer)
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	
	WeaponMesh = ObjectInitializer.CreateDefaultSubobject<USkeletalMeshComponent>(this, TEXT("WeaponMesh"));
	WeaponMesh->CastShadow = true;
	WeaponMesh->SetCollisionEnabled(ECollisionEnabled::NoCollision);
	RootComponent = WeaponMesh;

	WeaponCollision = CreateDefaultSubobject<UBoxComponent>(TEXT("WeaponCollision"));
	WeaponCollision->SetBoxExtent(FVector(3.f, 3.f, 3.f));
	WeaponCollision->AttachTo(WeaponMesh, "DamageSocket");

	static ConstructorHelpers::FObjectFinder<UParticleSystem>ParticleAsset(TEXT("ParticleSystem'/Game/StarterContent/Particles/P_Explosion.P_Explosion'"));
	HitFX = ParticleAsset.Object;

}

void AMyTestWeaponActor::SetOwningPawm(ABasicCharacter* NewOwner)
{
	if (MyPawn != NewOwner)
	{
		MyPawn = NewOwner;
	}
}

void AMyTestWeaponActor::AttachMeshToPawn()
{
	if (MyPawn)
	{
		USkeletalMeshComponent* PawnMesh = MyPawn->GetSpecificPawnMesh();
		FName AttachPoint = MyPawn->GetWeaponAttachPoint();
		WeaponMesh->AttachTo(PawnMesh, AttachPoint);
	}
}

void AMyTestWeaponActor::OnEquip(const AMyTestWeaponActor* LastWeapon)
{
	AttachMeshToPawn();
}
// Called when the game starts or when spawned
void AMyTestWeaponActor::BeginPlay()
{
	Super::BeginPlay();

}

// Called every frame
void AMyTestWeaponActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AMyTestWeaponActor::NotifyActorBeginOverlap(AActor* OtherActor)
{
	if (OtherActor->IsA(AActor::StaticClass()) && MyPawn->isDuringAttack && OtherActor != MyPawn)
	{
		UGameplayStatics::ApplyDamage(OtherActor, 30.f, NULL, this, UDamageType::StaticClass());
		//GEngine->AddOnScreenDebugMessage(-1, 2.0f, FColor::Red, "ApplyDamage!");

		UGameplayStatics::SpawnEmitterAtLocation(GetWorld(), HitFX, GetActorLocation());
	}
}