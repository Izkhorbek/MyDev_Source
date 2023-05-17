// Fill out your copyright notice in the Description page of Project Settings.


#include "MyTestActor.h"
#include "Components/SphereComponent.h"
#include "BasicCharacter.h"
#include "Engine.h"
#include "MyTestCharacter.h"
#include "Kismet/GameplayStatics.h"

AMyTestActor::AMyTestActor()
{
	PrimaryActorTick.bCanEverTick = true;

	mStaticMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Obj"));
	RootComponent = mStaticMesh;
	
	CollisionSphere = CreateDefaultSubobject<USphereComponent>(TEXT("CollisionSphere"));
	CollisionSphere->InitSphereRadius(100.0f);
	CollisionSphere->SetupAttachment(RootComponent);

	CollisionSphere->OnComponentBeginOverlap.AddDynamic(this, &AMyTestActor::OnOverlanBegin);

	static ConstructorHelpers::FObjectFinder<UParticleSystem>ParticleAsset(TEXT("/Game/StarterContent/Particles/P_Explosion.P_Explosion"));
	ParticleFX = ParticleAsset.Object;

}

void AMyTestActor::BeginPlay()
{
	Super::BeginPlay();
}

void AMyTestActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void AMyTestActor::OnOverlanBegin(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	if (OtherActor->IsA(AMyTestCharacter::StaticClass()))
	{
		GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Red, TEXT("Attacked!"));

		UGameplayStatics::SpawnEmitterAtLocation(GetWorld(), ParticleFX, GetActorLocation());
		Destroy();
	}
}

