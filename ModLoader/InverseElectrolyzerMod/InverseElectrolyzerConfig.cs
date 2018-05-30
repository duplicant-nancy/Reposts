﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using TUNING;
using UnityEngine;


public class InverseElectrolyzerConfig : IBuildingConfig
{
	public const string ID = "InverseElectrolyzer";

	private const float FILTER_INPUT_RATE = 1f;

	private const float DIRTY_WATER_INPUT_RATE = 5f;

	private const float FILTER_CAPACITY = 1200f;

	private const float USED_FILTER_OUTPUT_RATE = 0.2f;

	private const float CLEAN_WATER_OUTPUT_RATE = 5f;

	private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
	{
		LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)
	};

	public override BuildingDef CreateBuildingDef()
	{
		string id = "InverseElectrolyzer";
		int width = 4;
		int height = 3;
		string anim = "waterpurifier_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] aLL_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tIER2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = SimViewMode.LiquidVentMap;
		buildingDef.MaterialCategory = MATERIALS.ALL_METALS;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		//go.AddOrGet<Pump>();
		//Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		//storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;

		go.AddOrGet<InverseElectrolyzer>();
		Prioritizable.AddRef(go);

		/*
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			new ElementConverter.ConsumedElement(new Tag("Filter"), 1f),
			new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 5f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[2]
		{
			new ElementConverter.OutputElement(5f, SimHashes.Water, 313.15f, true, 0f, 0.5f, false, 0.75f, 255, 0),
			new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 313.15f, true, 0f, 0.5f, false, 0.25f, 255, 0)
		};
		*/

		/*
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRate = 1f;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showInStatusPanel = false;
		elementConsumer.consumptionRadius = 2;
		*/


		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			new ElementConverter.ConsumedElement(new Tag("Oxygen"), 5*0.888f),
			new ElementConverter.ConsumedElement(new Tag("Hydrogen"), 5*0.111999989f)
		};		
		elementConverter.outputElements = new ElementConverter.OutputElement[1]
		{
			new ElementConverter.OutputElement(5f, SimHashes.Water, 313.15f, true, 0f, 0.5f, false, 0.75f, 255, 0)
		};
		/*
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = new Tag("ToxicSand");
		elementDropper.emitOffset = new Vector3(0f, 1f, 0f);

		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = new Tag("Filter");
		manualDeliveryKG.capacity = 1200f;
		manualDeliveryKG.refillMass = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;
		*/
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityKG = 2f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);//GameTags.Oxygen;
		conduitConsumer.forceAlwaysSatisfied = true;
		//conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		/*
		conduitDispenser.elementFilter = new SimHashes[1]
		{
			SimHashes.DirtyWater
		};
		*/
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject game_object)
		{
			PoweredActiveController.Instance instance = new PoweredActiveController.Instance(game_object.GetComponent<KPrefabID>());
			instance.StartSM();
		};
	}
}

