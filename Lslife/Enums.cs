using GTA.Native;

namespace LSlife
{
  public static class Enums
  {
    public static PedHash GetPolicePedType()
    {
      switch (LSL.areas[LSL.areaIndex].PoliceType)
      {
        case LSL.jurisdictionType.LSPD:
          return PedHash.Cop01SMY;
        case LSL.jurisdictionType.LSSD:
          return PedHash.Sheriff01SMY;
        case LSL.jurisdictionType.BCSO:
          return PedHash.Sheriff01SMY;
        case LSL.jurisdictionType.SAPR:
          return PedHash.Ranger01SMY;
        default:
          return PedHash.Cop01SMY;
      }
    }

    public static VehicleHash GetPoliceVehType()
    {
      switch (LSL.areas[LSL.areaIndex].PoliceType)
      {
        case LSL.jurisdictionType.LSPD:
          return VehicleHash.Police;
        case LSL.jurisdictionType.LSSD:
          return VehicleHash.Sheriff;
        case LSL.jurisdictionType.BCSO:
          return VehicleHash.Sheriff;
        case LSL.jurisdictionType.SAPR:
          return VehicleHash.Pranger;
        default:
          return VehicleHash.Police;
      }
    }

    public enum eTaskTypeIndex
    {
      CTaskHandsUp = 0,
      CTaskClimbLadder = 1,
      CTaskExitVehicle = 2,
      CTaskCombatRoll = 3,
      CTaskAimGunOnFoot = 4,
      CTaskMovePlayer = 5,
      CTaskPlayerOnFoot = 6,
      CTaskWeapon = 8,
      CTaskPlayerWeapon = 9,
      CTaskPlayerIdles = 10, // 0x0000000A
      CTaskAimGun = 12, // 0x0000000C
      CTaskComplex = 12, // 0x0000000C
      CTaskFSMClone = 12, // 0x0000000C
      CTaskMotionBase = 12, // 0x0000000C
      CTaskMove = 12, // 0x0000000C
      CTaskMoveBase = 12, // 0x0000000C
      CTaskNMBehaviour = 12, // 0x0000000C
      CTaskNavBase = 12, // 0x0000000C
      CTaskScenario = 12, // 0x0000000C
      CTaskSearchBase = 12, // 0x0000000C
      CTaskSearchInVehicleBase = 12, // 0x0000000C
      CTaskShockingEvent = 12, // 0x0000000C
      CTaskTrainBase = 12, // 0x0000000C
      CTaskVehicleFSM = 12, // 0x0000000C
      CTaskVehicleGoTo = 12, // 0x0000000C
      CTaskVehicleMissionBase = 12, // 0x0000000C
      CTaskVehicleTempAction = 12, // 0x0000000C
      CTaskPause = 14, // 0x0000000E
      CTaskDoNothing = 15, // 0x0000000F
      CTaskGetUp = 16, // 0x00000010
      CTaskGetUpAndStandStill = 17, // 0x00000011
      CTaskFallOver = 18, // 0x00000012
      CTaskFallAndGetUp = 19, // 0x00000013
      CTaskCrawl = 20, // 0x00000014
      CTaskComplexOnFire = 25, // 0x00000019
      CTaskDamageElectric = 26, // 0x0000001A
      CTaskTriggerLookAt = 28, // 0x0000001C
      CTaskClearLookAt = 29, // 0x0000001D
      CTaskSetCharDecisionMaker = 30, // 0x0000001E
      CTaskSetPedDefensiveArea = 31, // 0x0000001F
      CTaskUseSequence = 32, // 0x00000020
      CTaskMoveStandStill = 34, // 0x00000022
      CTaskComplexControlMovement = 35, // 0x00000023
      CTaskMoveSequence = 36, // 0x00000024
      CTaskAmbientClips = 38, // 0x00000026
      CTaskMoveInAir = 39, // 0x00000027
      CTaskNetworkClone = 40, // 0x00000028
      CTaskUseClimbOnRoute = 41, // 0x00000029
      CTaskUseDropDownOnRoute = 42, // 0x0000002A
      CTaskUseLadderOnRoute = 43, // 0x0000002B
      CTaskSetBlockingOfNonTemporaryEvents = 44, // 0x0000002C
      CTaskForceMotionState = 45, // 0x0000002D
      CTaskSlopeScramble = 46, // 0x0000002E
      CTaskGoToAndClimbLadder = 47, // 0x0000002F
      CTaskClimbLadderFully = 48, // 0x00000030
      CTaskRappel = 49, // 0x00000031
      CTaskVault = 50, // 0x00000032
      CTaskDropDown = 51, // 0x00000033
      CTaskAffectSecondaryBehaviour = 52, // 0x00000034
      CTaskAmbientLookAtEvent = 53, // 0x00000035
      CTaskOpenDoor = 54, // 0x00000036
      CTaskShovePed = 55, // 0x00000037
      CTaskSwapWeapon = 56, // 0x00000038
      CTaskGeneralSweep = 57, // 0x00000039
      CTaskPolice = 58, // 0x0000003A
      CTaskPoliceOrderResponse = 59, // 0x0000003B
      CTaskPursueCriminal = 60, // 0x0000003C
      CTaskArrestPed = 62, // 0x0000003E
      CTaskArrestPed2 = 63, // 0x0000003F
      CTaskBusted = 64, // 0x00000040
      CTaskFirePatrol = 65, // 0x00000041
      CTaskHeliOrderResponse = 66, // 0x00000042
      CTaskHeliPassengerRappel = 67, // 0x00000043
      CTaskAmbulancePatrol = 68, // 0x00000044
      CTaskPoliceWantedResponse = 69, // 0x00000045
      CTaskSwat = 70, // 0x00000046
      CTaskSwatWantedResponse = 72, // 0x00000048
      CTaskSwatOrderResponse = 73, // 0x00000049
      CTaskSwatGoToStagingArea = 74, // 0x0000004A
      CTaskSwatFollowInLine = 75, // 0x0000004B
      CTaskWitness = 76, // 0x0000004C
      CTaskGangPatrol = 77, // 0x0000004D
      CTaskArmy = 78, // 0x0000004E
      CTaskShockingEventWatch = 80, // 0x00000050
      CTaskShockingEventGoto = 82, // 0x00000052
      CTaskShockingEventHurryAway = 83, // 0x00000053
      CTaskShockingEventReactToAircraft = 84, // 0x00000054
      CTaskShockingEventReact = 85, // 0x00000055
      CTaskShockingEventBackAway = 86, // 0x00000056
      CTaskShockingPoliceInvestigate = 87, // 0x00000057
      CTaskShockingEventStopAndStare = 88, // 0x00000058
      CTaskShockingNiceCarPicture = 89, // 0x00000059
      CTaskShockingEventThreatResponse = 90, // 0x0000005A
      CTaskTakeOffHelmet = 92, // 0x0000005C
      CTaskCarReactToVehicleCollision = 93, // 0x0000005D
      CTaskCarReactToVehicleCollisionGetOut = 95, // 0x0000005F
      CTaskDyingDead = 97, // 0x00000061
      CTaskWanderingScenario = 100, // 0x00000064
      CTaskWanderingInRadiusScenario = 101, // 0x00000065
      CTaskMoveBetweenPointsScenario = 103, // 0x00000067
      CTaskChatScenario = 104, // 0x00000068
      CTaskCowerScenario = 106, // 0x0000006A
      CTaskDeadBodyScenario = 107, // 0x0000006B
      CTaskSayAudio = 114, // 0x00000072
      CTaskWaitForSteppingOut = 116, // 0x00000074
      CTaskCoupleScenario = 117, // 0x00000075
      CTaskUseScenario = 118, // 0x00000076
      CTaskUseVehicleScenario = 119, // 0x00000077
      CTaskUnalerted = 120, // 0x00000078
      CTaskStealVehicle = 121, // 0x00000079
      CTaskReactToPursuit = 122, // 0x0000007A
      CTaskHitWall = 125, // 0x0000007D
      CTaskCower = 126, // 0x0000007E
      CTaskCrouch = 127, // 0x0000007F
      CTaskMelee = 128, // 0x00000080
      CTaskMoveMeleeMovement = 129, // 0x00000081
      CTaskMeleeActionResult = 130, // 0x00000082
      CTaskMeleeUpperbodyAnims = 131, // 0x00000083
      CTaskMoVEScripted = 133, // 0x00000085
      CTaskScriptedAnimation = 134, // 0x00000086
      CTaskSynchronizedScene = 135, // 0x00000087
      CTaskComplexEvasiveStep = 137, // 0x00000089
      CTaskWalkRoundCarWhileWandering = 138, // 0x0000008A
      CTaskComplexStuckInAir = 140, // 0x0000008C
      CTaskWalkRoundEntity = 141, // 0x0000008D
      CTaskMoveWalkRoundVehicle = 142, // 0x0000008E
      CTaskReactToGunAimedAt = 144, // 0x00000090
      CTaskDuckAndCover = 146, // 0x00000092
      CTaskAggressiveRubberneck = 147, // 0x00000093
      CTaskInVehicleBasic = 150, // 0x00000096
      CTaskCarDriveWander = 151, // 0x00000097
      CTaskLeaveAnyCar = 152, // 0x00000098
      CTaskComplexGetOffBoat = 153, // 0x00000099
      CTaskCarSetTempAction = 155, // 0x0000009B
      CTaskBringVehicleToHalt = 156, // 0x0000009C
      CTaskCarDrive = 157, // 0x0000009D
      CTaskPlayerDrive = 159, // 0x0000009F
      CTaskEnterVehicle = 160, // 0x000000A0
      CTaskEnterVehicleAlign = 161, // 0x000000A1
      CTaskOpenVehicleDoorFromOutside = 162, // 0x000000A2
      CTaskEnterVehicleSeat = 163, // 0x000000A3
      CTaskCloseVehicleDoorFromInside = 164, // 0x000000A4
      CTaskInVehicleSeatShuffle = 165, // 0x000000A5
      CTaskExitVehicleSeat = 167, // 0x000000A7
      CTaskCloseVehicleDoorFromOutside = 168, // 0x000000A8
      CTaskControlVehicle = 169, // 0x000000A9
      CTaskMotionInAutomobile = 170, // 0x000000AA
      CTaskMotionOnBicycle = 171, // 0x000000AB
      CTaskMotionOnBicycleController = 172, // 0x000000AC
      CTaskMotionInVehicle = 173, // 0x000000AD
      CTaskMotionInTurret = 174, // 0x000000AE
      CTaskReactToBeingJacked = 175, // 0x000000AF
      CTaskReactToBeingAskedToLeaveVehicle = 176, // 0x000000B0
      CTaskTryToGrabVehicleDoor = 177, // 0x000000B1
      CTaskGetOnTrain = 178, // 0x000000B2
      CTaskGetOffTrain = 179, // 0x000000B3
      CTaskRideTrain = 180, // 0x000000B4
      CTaskMountThrowProjectile = 190, // 0x000000BE
      CTaskGoToCarDoorAndStandStill = 195, // 0x000000C3
      CTaskMoveGoToVehicleDoor = 196, // 0x000000C4
      CTaskSetPedInVehicle = 197, // 0x000000C5
      CTaskSetPedOutOfVehicle = 198, // 0x000000C6
      CTaskVehicleMountedWeapon = 199, // 0x000000C7
      CTaskVehicleGun = 200, // 0x000000C8
      CTaskVehicleProjectile = 201, // 0x000000C9
      CTaskSmashCarWindow = 204, // 0x000000CC
      CTaskMoveGoToPoint = 205, // 0x000000CD
      CTaskMoveAchieveHeading = 206, // 0x000000CE
      CTaskMoveFaceTarget = 207, // 0x000000CF
      CTaskComplexGoToPointAndStandStillTimed = 208, // 0x000000D0
      CTaskMoveGoToPointAndStandStill = 208, // 0x000000D0
      CTaskMoveFollowPointRoute = 209, // 0x000000D1
      CTaskMoveSeekEntity_CEntitySeekPosCalculatorStandard = 210, // 0x000000D2
      CTaskMoveSeekEntity_CEntitySeekPosCalculatorLastNavMeshIntersection = 211, // 0x000000D3
      CTaskMoveSeekEntity_CEntitySeekPosCalculatorLastNavMeshIntersection2 = 212, // 0x000000D4
      CTaskMoveSeekEntity_CEntitySeekPosCalculatorXYOffsetFixed = 213, // 0x000000D5
      CTaskMoveSeekEntity_CEntitySeekPosCalculatorXYOffsetFixed2 = 214, // 0x000000D6
      CTaskExhaustedFlee = 215, // 0x000000D7
      CTaskGrowlAndFlee = 216, // 0x000000D8
      CTaskScenarioFlee = 217, // 0x000000D9
      CTaskSmartFlee = 218, // 0x000000DA
      CTaskFlyAway = 219, // 0x000000DB
      CTaskWalkAway = 220, // 0x000000DC
      CTaskWander = 221, // 0x000000DD
      CTaskWanderInArea = 222, // 0x000000DE
      CTaskFollowLeaderInFormation = 223, // 0x000000DF
      CTaskGoToPointAnyMeans = 224, // 0x000000E0
      CTaskTurnToFaceEntityOrCoord = 225, // 0x000000E1
      CTaskFollowLeaderAnyMeans = 226, // 0x000000E2
      CTaskFlyToPoint = 228, // 0x000000E4
      CTaskFlyingWander = 229, // 0x000000E5
      CTaskGoToPointAiming = 230, // 0x000000E6
      CTaskGoToScenario = 231, // 0x000000E7
      CTaskSeekEntityAiming = 233, // 0x000000E9
      CTaskSlideToCoord = 234, // 0x000000EA
      CTaskSwimmingWander = 235, // 0x000000EB
      CTaskMoveTrackingEntity = 237, // 0x000000ED
      CTaskMoveFollowNavMesh = 238, // 0x000000EE
      CTaskMoveGoToPointOnRoute = 239, // 0x000000EF
      CTaskEscapeBlast = 240, // 0x000000F0
      CTaskMoveWander = 241, // 0x000000F1
      CTaskMoveBeInFormation = 242, // 0x000000F2
      CTaskMoveCrowdAroundLocation = 243, // 0x000000F3
      CTaskMoveCrossRoadAtTrafficLights = 244, // 0x000000F4
      CTaskMoveWaitForTraffic = 245, // 0x000000F5
      CTaskMoveGoToPointStandStillAchieveHeading = 246, // 0x000000F6
      CTaskMoveGetOntoMainNavMesh = 251, // 0x000000FB
      CTaskMoveSlideToCoord = 252, // 0x000000FC
      CTaskMoveGoToPointRelativeToEntityAndStandStill = 253, // 0x000000FD
      CTaskHelicopterStrafe = 254, // 0x000000FE
      CTaskGetOutOfWater = 256, // 0x00000100
      CTaskMoveFollowEntityOffset = 259, // 0x00000103
      CTaskFollowWaypointRecording = 261, // 0x00000105
      CTaskMotionPed = 264, // 0x00000108
      CTaskMotionPedLowLod = 265, // 0x00000109
      CTaskHumanLocomotion = 268, // 0x0000010C
      CTaskMotionBasicLocomotionLowLod = 269, // 0x0000010D
      CTaskMotionStrafing = 270, // 0x0000010E
      CTaskMotionTennis = 271, // 0x0000010F
      CTaskMotionAiming = 272, // 0x00000110
      CTaskBirdLocomotion = 273, // 0x00000111
      CTaskFlightlessBirdLocomotion = 274, // 0x00000112
      CTaskFishLocomotion = 278, // 0x00000116
      CTaskQuadLocomotion = 279, // 0x00000117
      CTaskMotionDiving = 280, // 0x00000118
      CTaskMotionSwimming = 281, // 0x00000119
      CTaskMotionParachuting = 282, // 0x0000011A
      CTaskMotionDrunk = 283, // 0x0000011B
      CTaskRepositionMove = 284, // 0x0000011C
      CTaskMotionAimingTransition = 285, // 0x0000011D
      CTaskThrowProjectile = 286, // 0x0000011E
      CTaskCover = 287, // 0x0000011F
      CTaskMotionInCover = 288, // 0x00000120
      CTaskAimAndThrowProjectile = 289, // 0x00000121
      CTaskGun = 290, // 0x00000122
      CTaskAimFromGround = 291, // 0x00000123
      CTaskAimGunVehicleDriveBy = 295, // 0x00000127
      CTaskAimGunScripted = 296, // 0x00000128
      CTaskReloadGun = 298, // 0x0000012A
      CTaskWeaponBlocked = 299, // 0x0000012B
      CTaskEnterCover = 300, // 0x0000012C
      CTaskExitCover = 301, // 0x0000012D
      CTaskAimGunFromCoverIntro = 302, // 0x0000012E
      CTaskAimGunFromCoverOutro = 303, // 0x0000012F
      CTaskAimGunBlindFire = 304, // 0x00000130
      CTaskCombatClosestTargetInArea = 307, // 0x00000133
      CTaskCombatAdditionalTask = 308, // 0x00000134
      CTaskInCover = 309, // 0x00000135
      CTaskAimSweep = 313, // 0x00000139
      CTaskSharkCircle = 319, // 0x0000013F
      CTaskSharkAttack = 320, // 0x00000140
      CTaskAgitated = 321, // 0x00000141
      CTaskAgitatedAction = 322, // 0x00000142
      CTaskConfront = 323, // 0x00000143
      CTaskIntimidate = 324, // 0x00000144
      CTaskShove = 325, // 0x00000145
      CTaskShoved = 326, // 0x00000146
      CTaskCrouchToggle = 328, // 0x00000148
      CTaskRevive = 329, // 0x00000149
      CTaskParachute = 335, // 0x0000014F
      CTaskParachuteObject = 336, // 0x00000150
      CTaskTakeOffPedVariation = 337, // 0x00000151
      CTaskCombatSeekCover = 340, // 0x00000154
      CTaskCombatFlank = 342, // 0x00000156
      CTaskCombat = 343, // 0x00000157
      CTaskCombatMounted = 344, // 0x00000158
      CTaskMoveCircle = 345, // 0x00000159
      CTaskMoveCombatMounted = 346, // 0x0000015A
      CTaskSearch = 347, // 0x0000015B
      CTaskSearchOnFoot = 348, // 0x0000015C
      CTaskSearchInAutomobile = 349, // 0x0000015D
      CTaskSearchInBoat = 350, // 0x0000015E
      CTaskSearchInHeli = 351, // 0x0000015F
      CTaskThreatResponse = 352, // 0x00000160
      CTaskInvestigate = 353, // 0x00000161
      CTaskStandGuardFSM = 354, // 0x00000162
      CTaskPatrol = 355, // 0x00000163
      CTaskShootAtTarget = 356, // 0x00000164
      CTaskSetAndGuardArea = 357, // 0x00000165
      CTaskStandGuard = 358, // 0x00000166
      CTaskSeparate = 359, // 0x00000167
      CTaskStayInCover = 360, // 0x00000168
      CTaskVehicleCombat = 361, // 0x00000169
      CTaskVehiclePersuit = 362, // 0x0000016A
      CTaskVehicleChase = 363, // 0x0000016B
      CTaskDraggingToSafety = 364, // 0x0000016C
      CTaskDraggedToSafety = 365, // 0x0000016D
      CTaskVariedAimPose = 366, // 0x0000016E
      CTaskMoveWithinAttackWindow = 367, // 0x0000016F
      CTaskMoveWithinDefensiveArea = 368, // 0x00000170
      CTaskShootOutTire = 369, // 0x00000171
      CTaskShellShocked = 370, // 0x00000172
      CTaskBoatChase = 371, // 0x00000173
      CTaskBoatCombat = 372, // 0x00000174
      CTaskBoatStrafe = 373, // 0x00000175
      CTaskHeliChase = 374, // 0x00000176
      CTaskHeliCombat = 375, // 0x00000177
      CTaskSubmarineCombat = 376, // 0x00000178
      CTaskSubmarineChase = 377, // 0x00000179
      CTaskPlaneChase = 378, // 0x0000017A
      CTaskTargetUnreachable = 379, // 0x0000017B
      CTaskTargetUnreachableInInterior = 380, // 0x0000017C
      CTaskTargetUnreachableInExterior = 381, // 0x0000017D
      CTaskStealthKill = 382, // 0x0000017E
      CTaskWrithe = 383, // 0x0000017F
      CTaskAdvance = 384, // 0x00000180
      CTaskCharge = 385, // 0x00000181
      CTaskMoveToTacticalPoint = 386, // 0x00000182
      CTaskToHurtTransit = 387, // 0x00000183
      CTaskAnimatedHitByExplosion = 388, // 0x00000184
      CTaskNMRelax = 389, // 0x00000185
      CTaskNMPose = 391, // 0x00000187
      CTaskNMBrace = 392, // 0x00000188
      CTaskNMBuoyancy = 393, // 0x00000189
      CTaskNMInjuredOnGround = 394, // 0x0000018A
      CTaskNMShot = 395, // 0x0000018B
      CTaskNMHighFall = 396, // 0x0000018C
      CTaskNMBalance = 397, // 0x0000018D
      CTaskNMElectrocute = 398, // 0x0000018E
      CTaskNMPrototype = 399, // 0x0000018F
      CTaskNMExplosion = 400, // 0x00000190
      CTaskNMOnFire = 401, // 0x00000191
      CTaskNMScriptControl = 402, // 0x00000192
      CTaskNMJumpRollFromRoadVehicle = 403, // 0x00000193
      CTaskNMFlinch = 404, // 0x00000194
      CTaskNMSit = 405, // 0x00000195
      CTaskNMFallDown = 406, // 0x00000196
      CTaskBlendFromNM = 407, // 0x00000197
      CTaskNMControl = 408, // 0x00000198
      CTaskNMDangle = 409, // 0x00000199
      CTaskNMGenericAttach = 412, // 0x0000019C
      CTaskNMDraggingToSafety = 414, // 0x0000019E
      CTaskNMThroughWindscreen = 415, // 0x0000019F
      CTaskNMRiverRapids = 416, // 0x000001A0
      CTaskNMSimple = 417, // 0x000001A1
      CTaskRageRagdoll = 418, // 0x000001A2
      CTaskJumpVault = 421, // 0x000001A5
      CTaskJump = 422, // 0x000001A6
      CTaskFall = 423, // 0x000001A7
      CTaskReactAimWeapon = 425, // 0x000001A9
      CTaskChat = 426, // 0x000001AA
      CTaskMobilePhone = 427, // 0x000001AB
      CTaskReactToDeadPed = 428, // 0x000001AC
      CTaskSearchForUnknownThreat = 430, // 0x000001AE
      CTaskBomb = 432, // 0x000001B0
      CTaskDetonator = 433, // 0x000001B1
      CTaskAnimatedAttach = 435, // 0x000001B3
      CTaskCutScene = 441, // 0x000001B9
      CTaskReactToExplosion = 442, // 0x000001BA
      CTaskReactToImminentExplosion = 443, // 0x000001BB
      CTaskDiveToGround = 444, // 0x000001BC
      CTaskReactAndFlee = 445, // 0x000001BD
      CTaskSidestep = 446, // 0x000001BE
      CTaskCallPolice = 447, // 0x000001BF
      CTaskReactInDirection = 448, // 0x000001C0
      CTaskReactToBuddyShot = 449, // 0x000001C1
      CTaskVehicleGoToAutomobileNew = 454, // 0x000001C6
      CTaskVehicleGoToPlane = 455, // 0x000001C7
      CTaskVehicleGoToHelicopter = 456, // 0x000001C8
      CTaskVehicleGoToSubmarine = 457, // 0x000001C9
      CTaskVehicleGoToBoat = 458, // 0x000001CA
      CTaskVehicleGoToPointAutomobile = 459, // 0x000001CB
      CTaskVehicleGoToPointWithAvoidanceAutomobile = 460, // 0x000001CC
      CTaskVehiclePursue = 461, // 0x000001CD
      CTaskVehicleRam = 462, // 0x000001CE
      CTaskVehicleSpinOut = 463, // 0x000001CF
      CTaskVehicleApproach = 464, // 0x000001D0
      CTaskVehicleThreePointTurn = 465, // 0x000001D1
      CTaskVehicleDeadDriver = 466, // 0x000001D2
      CTaskVehicleCruiseNew = 467, // 0x000001D3
      CTaskVehicleCruiseBoat = 468, // 0x000001D4
      CTaskVehicleStop = 469, // 0x000001D5
      CTaskVehiclePullOver = 470, // 0x000001D6
      CTaskVehiclePassengerExit = 471, // 0x000001D7
      CTaskVehicleFlee = 472, // 0x000001D8
      CTaskVehicleFleeAirborne = 473, // 0x000001D9
      CTaskVehicleFleeBoat = 474, // 0x000001DA
      CTaskVehicleFollowRecording = 475, // 0x000001DB
      CTaskVehicleFollow = 476, // 0x000001DC
      CTaskVehicleBlock = 477, // 0x000001DD
      CTaskVehicleBlockBackAndForth = 478, // 0x000001DE
      CTaskVehicleBlockCruiseInFront = 478, // 0x000001DE
      CTaskVehicleBlockBrakeInFront = 479, // 0x000001DF
      CTaskVehicleCrash = 481, // 0x000001E1
      CTaskVehicleLand = 482, // 0x000001E2
      CTaskVehicleLandPlane = 483, // 0x000001E3
      CTaskVehicleHover = 484, // 0x000001E4
      CTaskVehicleAttack = 485, // 0x000001E5
      CTaskVehicleAttackTank = 486, // 0x000001E6
      CTaskVehicleCircle = 487, // 0x000001E7
      CTaskVehiclePoliceBehaviour = 488, // 0x000001E8
      CTaskVehiclePoliceBehaviourHelicopter = 489, // 0x000001E9
      CTaskVehiclePoliceBehaviourBoat = 490, // 0x000001EA
      CTaskVehicleEscort = 491, // 0x000001EB
      CTaskVehicleHeliProtect = 492, // 0x000001EC
      CTaskVehiclePlayerDriveAutomobile = 494, // 0x000001EE
      CTaskVehiclePlayerDriveBike = 495, // 0x000001EF
      CTaskVehiclePlayerDriveBoat = 496, // 0x000001F0
      CTaskVehiclePlayerDriveSubmarine = 497, // 0x000001F1
      CTaskVehiclePlayerDriveSubmarineCar = 498, // 0x000001F2
      CTaskVehiclePlayerDriveAmphibiousAutomobile = 499, // 0x000001F3
      CTaskVehiclePlayerDrivePlane = 500, // 0x000001F4
      CTaskVehiclePlayerDriveHeli = 501, // 0x000001F5
      CTaskVehiclePlayerDriveAutogyro = 502, // 0x000001F6
      CTaskVehiclePlayerDriveDiggerArm = 503, // 0x000001F7
      CTaskVehiclePlayerDriveTrain = 504, // 0x000001F8
      CTaskVehiclePlaneChase = 505, // 0x000001F9
      CTaskVehicleNoDriver = 506, // 0x000001FA
      CTaskVehicleAnimation = 507, // 0x000001FB
      CTaskVehicleConvertibleRoof = 508, // 0x000001FC
      CTaskVehicleParkNew = 509, // 0x000001FD
      CTaskVehicleFollowWaypointRecording = 510, // 0x000001FE
      CTaskVehicleGoToNavmesh = 511, // 0x000001FF
      CTaskVehicleReactToCopSiren = 512, // 0x00000200
      CTaskVehicleGotoLongRange = 513, // 0x00000201
      CTaskVehicleWait = 514, // 0x00000202
      CTaskVehicleReverse = 515, // 0x00000203
      CTaskVehicleBrake = 516, // 0x00000204
      CTaskVehicleHandBrake = 517, // 0x00000205
      CTaskVehicleTurn = 518, // 0x00000206
      CTaskVehicleGoForward = 519, // 0x00000207
      CTaskVehicleSwerve = 520, // 0x00000208
      CTaskVehicleFlyDirection = 521, // 0x00000209
      CTaskVehicleHeadonCollision = 522, // 0x0000020A
      CTaskVehicleBoostUseSteeringAngle = 523, // 0x0000020B
      CTaskVehicleShotTire = 524, // 0x0000020C
      CTaskVehicleBurnout = 525, // 0x0000020D
      CTaskVehicleRevEngine = 526, // 0x0000020E
      CTaskVehicleSurfaceInSubmarine = 527, // 0x0000020F
      CTaskVehiclePullAlongside = 528, // 0x00000210
      CTaskVehicleTransformToSubmarine = 529, // 0x00000211
      CTaskAnimatedFallback = 530, // 0x00000212
    }

    public enum DealState
    {
      GetNear,
      Approach,
      InPosition,
      DoingDeal,
      WalkingAway,
    }

    public enum HealPositionTypes
    {
      None,
      House,
      Hospital,
    }

    public enum PdStates
    {
      NoState,
      Work,
      Dealing,
      StopForPlayer,
      LookAtPlayer,
      TalkToPlayer,
      WalkToPath,
      Dead,
      FollowPlayer,
      FollowingPlayer,
      Rest,
      Resting,
      Defend,
      LeadTeam,
      JoinTeam,
    }

    public enum DStates
    {
      NoState,
      Normal,
      Dealing,
      StopForPlayer,
      LookAtPlayer,
      TalkToPlayer,
      AttackPlayer,
      WalkToPath,
    }

    public enum DriveBySates
    {
      VehDrive,
      VehPatrol,
      VehAttack,
      CleanDriveBy,
      VehFollow,
    }

    public enum LawEnforcePed
    {
      Police = 6,
      Swat = 27, // 0x0000001B
      Army = 29, // 0x0000001D
    }

    public enum DayGTA
    {
      Sunday,
      Monday,
      Tuesday,
      Wednesday,
      Thursday,
      Friday,
      Saturday,
    }

    public enum PedType
    {
      PED_TYPE_PLAYER_0,
      PED_TYPE_PLAYER_1,
      PED_TYPE_NETWORK_PLAYER,
      PED_TYPE_PLAYER_2,
      PED_TYPE_CIVMALE,
      PED_TYPE_CIVFEMALE,
      PED_TYPE_COP,
      PED_TYPE_GANG_ALBANIAN,
      PED_TYPE_GANG_BIKER_1,
      PED_TYPE_COPTYPE_GANG_BIKER_2,
      PED_TYPE_GANG_ITALIAN,
      PED_TYPE_GANG_RUSSIAN,
      PED_TYPE_COPTYPE_GANG_RUSSIAN_2,
      PED_TYPE_GANG_IRISH,
      PED_TYPE_GANG_JAMAICAN,
      PED_TYPE_GANG_AFRICAN_AMERICAN,
      PED_TYPE_GANG_KOREAN,
      PED_TYPE_GANG_CHINESE_JAPANESE,
      PED_TYPE_GANG_PUERTO_RICAN,
      PED_TYPE_DEALER,
      PED_TYPE_MEDIC,
      PED_TYPE_FIREMAN,
      PED_TYPE_CRIMINAL,
      PED_TYPE_BUM,
      PED_TYPE_PROSTITUTE,
      PED_TYPE_SPECIAL,
      PED_TYPE_MISSION,
      PED_TYPE_SWAT,
      PED_TYPE_ANIMAL,
      PED_TYPE_ARMY,
    }

    private enum EventType
    {
      EVENT_INVALID = -1, // 0xFFFFFFFF
      EVENT_ACQUAINTANCE_PED_DISLIKE = 0,
      EVENT_ACQUAINTANCE_PED_HATE = 1,
      EVENT_ACQUAINTANCE_PED_LIKE = 2,
      EVENT_ACQUAINTANCE_PED_RESPECT = 3,
      EVENT_ACQUAINTANCE_PED_WANTED = 4,
      EVENT_ACQUAINTANCE_PED_DEAD = 5,
      EVENT_AGITATED = 6,
      EVENT_AGITATED_ACTION = 7,
      EVENT_ENCROACHING_PED = 8,
      EVENT_0x81F22B52 = 9,
      EVENT_0x60CB8FA0 = 10, // 0x0000000A
      EVENT_0x82E1AF98 = 11, // 0x0000000B
      EVENT_0x3BD3D2C2 = 12, // 0x0000000C
      EVENT_0xF2B054F1 = 13, // 0x0000000D
      EVENT_COMMUNICATE_EVENT = 14, // 0x0000000E
      EVENT_COP_CAR_BEING_STOLEN = 15, // 0x0000000F
      EVENT_CRIME_REPORTED = 16, // 0x00000010
      EVENT_DAMAGE = 17, // 0x00000011
      EVENT_DEAD_PED_FOUND = 18, // 0x00000012
      EVENT_0xB1A36980 = 19, // 0x00000013
      EVENT_DRAGGED_OUT_CAR = 20, // 0x00000014
      EVENT_DUMMY_CONVERSION = 21, // 0x00000015
      EVENT_EXPLOSION = 22, // 0x00000016
      EVENT_EXPLOSION_HEARD = 23, // 0x00000017
      EVENT_FIRE_NEARBY = 24, // 0x00000018
      EVENT_0x7F696887 = 25, // 0x00000019
      EVENT_FOOT_STEP_HEARD = 26, // 0x0000001A
      EVENT_GET_OUT_OF_WATER = 27, // 0x0000001B
      EVENT_GIVE_PED_TASK = 28, // 0x0000001C
      EVENT_GUN_AIMED_AT = 29, // 0x0000001D
      EVENT_0xEB411462 = 30, // 0x0000001E
      EVENT_INJURED_CRY_FOR_HELP = 31, // 0x0000001F
      EVENT_CRIME_CRY_FOR_HELP = 32, // 0x00000020
      EVENT_IN_AIR = 33, // 0x00000021
      EVENT_IN_WATER = 34, // 0x00000022
      EVENT_INCAPACITATED = 35, // 0x00000023
      EVENT_LEADER_ENTERED_CAR_AS_DRIVER = 36, // 0x00000024
      EVENT_LEADER_ENTERED_COVER = 37, // 0x00000025
      EVENT_LEADER_EXITED_CAR_AS_DRIVER = 38, // 0x00000026
      EVENT_LEADER_HOLSTERED_WEAPON = 39, // 0x00000027
      EVENT_LEADER_LEFT_COVER = 40, // 0x00000028
      EVENT_LEADER_UNHOLSTERED_WEAPON = 41, // 0x00000029
      EVENT_MELEE_ACTION = 42, // 0x0000002A
      EVENT_MUST_LEAVE_BOAT = 43, // 0x0000002B
      EVENT_NEW_TASK = 44, // 0x0000002C
      EVENT_NONE = 45, // 0x0000002D
      EVENT_OBJECT_COLLISION = 46, // 0x0000002E
      EVENT_ON_FIRE = 47, // 0x0000002F
      EVENT_OPEN_DOOR = 48, // 0x00000030
      EVENT_SHOVE_PED = 49, // 0x00000031
      EVENT_PED_COLLISION_WITH_PED = 50, // 0x00000032
      EVENT_PED_COLLISION_WITH_PLAYER = 51, // 0x00000033
      EVENT_PED_ENTERED_MY_VEHICLE = 52, // 0x00000034
      EVENT_PED_JACKING_MY_VEHICLE = 53, // 0x00000035
      EVENT_0xB74989BD = 54, // 0x00000036
      EVENT_0x01F83885 = 55, // 0x00000037
      EVENT_0x3CB38653 = 56, // 0x00000038
      EVENT_PLAYER_COLLISION_WITH_PED = 57, // 0x00000039
      EVENT_0x7BA020DA = 58, // 0x0000003A
      EVENT_POTENTIAL_BE_WALKED_INTO = 59, // 0x0000003B
      EVENT_POTENTIAL_BLAST = 60, // 0x0000003C
      EVENT_POTENTIAL_GET_RUN_OVER = 61, // 0x0000003D
      EVENT_POTENTIAL_WALK_INTO_FIRE = 62, // 0x0000003E
      EVENT_POTENTIAL_WALK_INTO_OBJECT = 63, // 0x0000003F
      EVENT_POTENTIAL_WALK_INTO_VEHICLE = 64, // 0x00000040
      EVENT_PROVIDING_COVER = 65, // 0x00000041
      EVENT_RADIO_TARGET_POSITION = 66, // 0x00000042
      EVENT_RAN_OVER_PED = 67, // 0x00000043
      EVENT_0xA81BCDE5 = 68, // 0x00000044
      EVENT_0xEF9F926C = 69, // 0x00000045
      EVENT_0x2304F532 = 70, // 0x00000046
      EVENT_0x366990F6 = 71, // 0x00000047
      EVENT_REQUEST_HELP_WITH_CONFRONTATION = 72, // 0x00000048
      EVENT_RESPONDED_TO_THREAT = 73, // 0x00000049
      EVENT_REVIVED = 74, // 0x0000004A
      EVENT_SCRIPT_COMMAND = 75, // 0x0000004B
      EVENT_SHOCKING_BROKEN_GLASS = 76, // 0x0000004C
      EVENT_SHOCKING_CAR_ALARM = 77, // 0x0000004D
      EVENT_SHOCKING_CAR_CHASE = 78, // 0x0000004E
      EVENT_SHOCKING_CAR_CRASH = 79, // 0x0000004F
      EVENT_SHOCKING_BICYCLE_CRASH = 80, // 0x00000050
      EVENT_SHOCKING_CAR_PILE_UP = 81, // 0x00000051
      EVENT_SHOCKING_CAR_ON_CAR = 82, // 0x00000052
      EVENT_SHOCKING_DANGEROUS_ANIMAL = 83, // 0x00000053
      EVENT_SHOCKING_DEAD_BODY = 84, // 0x00000054
      EVENT_SHOCKING_DRIVING_ON_PAVEMENT = 85, // 0x00000055
      EVENT_SHOCKING_BICYCLE_ON_PAVEMENT = 86, // 0x00000056
      EVENT_SHOCKING_ENGINE_REVVED = 87, // 0x00000057
      EVENT_SHOCKING_EXPLOSION = 88, // 0x00000058
      EVENT_SHOCKING_FIRE = 89, // 0x00000059
      EVENT_SHOCKING_GUN_FIGHT = 90, // 0x0000005A
      EVENT_SHOCKING_GUNSHOT_FIRED = 91, // 0x0000005B
      EVENT_SHOCKING_HELICOPTER_OVERHEAD = 92, // 0x0000005C
      EVENT_SHOCKING_PARACHUTER_OVERHEAD = 93, // 0x0000005D
      EVENT_SHOCKING_PED_KNOCKED_INTO_BY_PLAYER = 94, // 0x0000005E
      EVENT_SHOCKING_HORN_SOUNDED = 95, // 0x0000005F
      EVENT_SHOCKING_IN_DANGEROUS_VEHICLE = 96, // 0x00000060
      EVENT_SHOCKING_INJURED_PED = 97, // 0x00000061
      EVENT_SHOCKING_MAD_DRIVER = 98, // 0x00000062
      EVENT_SHOCKING_MAD_DRIVER_EXTREME = 99, // 0x00000063
      EVENT_SHOCKING_MAD_DRIVER_BICYCLE = 100, // 0x00000064
      EVENT_SHOCKING_MUGGING = 101, // 0x00000065
      EVENT_SHOCKING_NON_VIOLENT_WEAPON_AIMED_AT = 102, // 0x00000066
      EVENT_SHOCKING_PED_RUN_OVER = 103, // 0x00000067
      EVENT_SHOCKING_PED_SHOT = 104, // 0x00000068
      EVENT_SHOCKING_PLANE_FLY_BY = 105, // 0x00000069
      EVENT_SHOCKING_POTENTIAL_BLAST = 106, // 0x0000006A
      EVENT_SHOCKING_PROPERTY_DAMAGE = 107, // 0x0000006B
      EVENT_SHOCKING_RUNNING_PED = 108, // 0x0000006C
      EVENT_SHOCKING_RUNNING_STAMPEDE = 109, // 0x0000006D
      EVENT_SHOCKING_SEEN_CAR_STOLEN = 110, // 0x0000006E
      EVENT_SHOCKING_SEEN_CONFRONTATION = 111, // 0x0000006F
      EVENT_SHOCKING_SEEN_GANG_FIGHT = 112, // 0x00000070
      EVENT_SHOCKING_SEEN_INSULT = 113, // 0x00000071
      EVENT_SHOCKING_SEEN_MELEE_ACTION = 114, // 0x00000072
      EVENT_SHOCKING_SEEN_NICE_CAR = 115, // 0x00000073
      EVENT_SHOCKING_SEEN_PED_KILLED = 116, // 0x00000074
      EVENT_SHOCKING_SEEN_VEHICLE_TOWED = 117, // 0x00000075
      EVENT_SHOCKING_SEEN_WEAPON_THREAT = 118, // 0x00000076
      EVENT_SHOCKING_SEEN_WEIRD_PED = 119, // 0x00000077
      EVENT_SHOCKING_SEEN_WEIRD_PED_APPROACHING = 120, // 0x00000078
      EVENT_SHOCKING_SIREN = 121, // 0x00000079
      EVENT_SHOCKING_STUDIO_BOMB = 122, // 0x0000007A
      EVENT_SHOCKING_VISIBLE_WEAPON = 123, // 0x0000007B
      EVENT_SHOT_FIRED = 124, // 0x0000007C
      EVENT_SHOT_FIRED_BULLET_IMPACT = 125, // 0x0000007D
      EVENT_SHOT_FIRED_WHIZZED_BY = 126, // 0x0000007E
      EVENT_FRIENDLY_AIMED_AT = 127, // 0x0000007F
      EVENT_FRIENDLY_FIRE_NEAR_MISS = 128, // 0x00000080
      EVENT_0xFC16D876 = 129, // 0x00000081
      EVENT_SHOUT_TARGET_POSITION = 130, // 0x00000082
      EVENT_0x82C87CA3 = 131, // 0x00000083
      EVENT_STUCK_IN_AIR = 132, // 0x00000084
      EVENT_SUSPICIOUS_ACTIVITY = 133, // 0x00000085
      EVENT_0xA84F246C = 134, // 0x00000086
      EVENT_UNIDENTIFIED_PED = 135, // 0x00000087
      EVENT_VEHICLE_COLLISION = 136, // 0x00000088
      EVENT_VEHICLE_DAMAGE_WEAPON = 137, // 0x00000089
      EVENT_VEHICLE_ON_FIRE = 138, // 0x0000008A
      EVENT_WHISTLING_HEARD = 139, // 0x0000008B
      EVENT_DISTURBANCE = 140, // 0x0000008C
      EVENT_ENTITY_DAMAGED = 141, // 0x0000008D
      EVENT_ENTITY_DESTROYED = 142, // 0x0000008E
      EVENT_0xA4E4FA06 = 143, // 0x0000008F
      EVENT_0x3743E203 = 144, // 0x00000090
      EVENT_PLAYER_UNABLE_TO_ENTER_VEHICLE = 145, // 0x00000091
      EVENT_0x6E80ACD1 = 146, // 0x00000092
      EVENT_0x1E9C83E7 = 147, // 0x00000093
      EVENT_PLAYER_DEATH = 148, // 0x00000094
      EVENT_PED_SEEN_DEAD_PED = 149, // 0x00000095
      EVENT_0xC92B98C8 = 150, // 0x00000096
      EVENT_0xA773C13A = 151, // 0x00000097
      EVENT_0x7FC68049 = 152, // 0x00000098
      EVENT_0x36B5F57E = 153, // 0x00000099
      EVENT_0xE70A7E9C = 154, // 0x0000009A
      EVENT_0x7BB0EC60 = 155, // 0x0000009B
      EVENT_0x13137441 = 156, // 0x0000009C
      EVENT_0x7823923E = 157, // 0x0000009D
      EVENT_0x1DA46B48 = 158, // 0x0000009E
      EVENT_0x57205379 = 159, // 0x0000009F
      EVENT_0xD7941AA2 = 160, // 0x000000A0
      EVENT_0xC523FA9E = 161, // 0x000000A1
      EVENT_0x9F396CB7 = 162, // 0x000000A2
      EVENT_0x8EFAEC43 = 163, // 0x000000A3
      EVENT_0x6BE93522 = 164, // 0x000000A4
      EVENT_0x35B9C2F6 = 165, // 0x000000A5
      EVENT_0x516F2606 = 166, // 0x000000A6
      EVENT_0x102A0521 = 167, // 0x000000A7
      EVENT_0x2C24E311 = 168, // 0x000000A8
      EVENT_0x53C98CF2 = 169, // 0x000000A9
      EVENT_0x5B32A776 = 170, // 0x000000AA
      EVENT_0x9155EBDB = 171, // 0x000000AB
      EVENT_0xB3DEFC77 = 172, // 0x000000AC
      EVENT_0xE7F5BE65 = 173, // 0x000000AD
      EVENT_NETWORK_SCRIPT_EVENT = 174, // 0x000000AE
      EVENT_0xC2949D06 = 175, // 0x000000AF
      EVENT_0xB3F4CE20 = 176, // 0x000000B0
      EVENT_0x515192E4 = 177, // 0x000000B1
      EVENT_NETWORK_NETWORK_BAIL = 178, // 0x000000B2
      EVENT_0x28D7D5CB = 179, // 0x000000B3
      EVENT_0xC8D473AE = 180, // 0x000000B4
      EVENT_0xCDD45E2B = 181, // 0x000000B5
      EVENT_0x2AED0110 = 182, // 0x000000B6
      EVENT_0xE476A2E5 = 183, // 0x000000B7
      EVENT_0xBF25281C = 184, // 0x000000B8
      EVENT_0x8F2C59B5 = 185, // 0x000000B9
      EVENT_0x19A13553 = 186, // 0x000000BA
      EVENT_0xF8BB6EED = 187, // 0x000000BB
      EVENT_0x272B4BB1 = 188, // 0x000000BC
      EVENT_0x03C0D0BB = 189, // 0x000000BD
      EVENT_0xC5857F28 = 190, // 0x000000BE
      EVENT_0x6B39C3C7 = 191, // 0x000000BF
      EVENT_0x680E9124 = 192, // 0x000000C0
      EVENT_0x084F397B = 193, // 0x000000C1
      EVENT_0x50DB843C = 194, // 0x000000C2
      EVENT_0xB654C626 = 195, // 0x000000C3
      EVENT_0x60064F81 = 196, // 0x000000C4
      EVENT_TEXT_MESSAGE_RECEIVED = 197, // 0x000000C5
      EVENT_0x5EB082EB = 198, // 0x000000C6
      EVENT_0xCBD918A0 = 199, // 0x000000C7
      EVENT_0xAFF32159 = 200, // 0x000000C8
      EVENT_NETWORK_PED_LEFT_BEHIND = 201, // 0x000000C9
      EVENT_0x9EE353D5 = 202, // 0x000000CA
      EVENT_0x084288B7 = 203, // 0x000000CB
      EVENT_0x9E50D3EB = 204, // 0x000000CC
      EVENT_0x0EBF7E88 = 205, // 0x000000CD
      EVENT_0xCDDB7BF9 = 206, // 0x000000CE
      EVENT_0x35060103 = 207, // 0x000000CF
      EVENT_0xC45B9C8D = 208, // 0x000000D0
      EVENT_0x8461B627 = 209, // 0x000000D1
      EVENT_0x9ACF6B43 = 210, // 0x000000D2
      EVENT_0x526A8D2B = 211, // 0x000000D3
      EVENT_0x06F9071A = 212, // 0x000000D4
      EVENT_0xEBC33AFE = 213, // 0x000000D5
      EVENT_0x38141B44 = 214, // 0x000000D6
      EVENT_0xE52648CE = 215, // 0x000000D7
      EVENT_0xBA04727E = 216, // 0x000000D8
      EVENT_0x5A87C62C = 217, // 0x000000D9
      EVENT_0x35869060 = 218, // 0x000000DA
      EVENT_0xF06BB198 = 219, // 0x000000DB
      EVENT_0x003F6DC4 = 220, // 0x000000DC
      EVENT_0x8E787C49 = 221, // 0x000000DD
      EVENT_0xEC6788FA = 222, // 0x000000DE
      EVENT_0xB48AD6A2 = 223, // 0x000000DF
      EVENT_0x05FEB16F = 224, // 0x000000E0
      EVENT_0x04112C59 = 225, // 0x000000E1
      EVENT_0x474EBFE0 = 226, // 0x000000E2
      EVENT_0xAF6C7C96 = 227, // 0x000000E3
      EVENT_0xC681DA85 = 228, // 0x000000E4
      EVENT_0x835D6183 = 229, // 0x000000E5
      EVENT_0x03A52AA7 = 230, // 0x000000E6
      EVENT_0xF31E4A2A = 231, // 0x000000E7
      EVENT_0xFFC4CC2B = 232, // 0x000000E8
      EVENT_0x06EF2524 = 233, // 0x000000E9
      EVENT_0x157869FC = 234, // 0x000000EA
      EVENT_0x115B8784 = 235, // 0x000000EB
      EVENT_0x93AC2785 = 236, // 0x000000EC
      EVENT_0x6D63129F = 237, // 0x000000ED
      EVENT_0xBC778D37 = 238, // 0x000000EE
      EVENT_0x4A8A5373 = 239, // 0x000000EF
      EVENT_ERRORS_UNKNOWN_ERROR = 240, // 0x000000F0
      EVENT_ERRORS_ARRAY_OVERFLOW = 241, // 0x000000F1
      EVENT_ERRORS_INSTRUCTION_LIMIT = 242, // 0x000000F2
      EVENT_ERRORS_STACK_OVERFLOW = 243, // 0x000000F3
      EVENT_0x063E563B = 244, // 0x000000F4
      EVENT_0x9DEA6A90 = 245, // 0x000000F5
    }
  }
}
