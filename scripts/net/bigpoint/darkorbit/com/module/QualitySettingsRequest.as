package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class QualitySettingsRequest implements IModule
   {
      private static var _instance:QualitySettingsRequest;
      
      public var qualityAttack:int = 0;
      
      public var qualityBackground:int = 0;
      
      public var qualityPresetting:int = 0;
      
      public var qualityCustomized:Boolean = false;
      
      public var qualityPOIzone:int = 0;
      
      public var qualityShip:int = 0;
      
      public var qualityEngine:int = 0;
      
      public var qualityExplosion:int = 0;
      
      public var qualityCollectables:int = 0;
      
      public var qualityEffect:int = 0;
      
      public function QualitySettingsRequest(param1:int = 0, param2:int = 0, param3:int = 0, param4:Boolean = false, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0, param9:int = 0, param10:int = 0)
      {
         super();
         this.qualityAttack = param1;
         this.qualityBackground = param2;
         this.qualityPresetting = param3;
         this.qualityCustomized = param4;
         this.qualityPOIzone = param5;
         this.qualityShip = param6;
         this.qualityEngine = param7;
         this.qualityExplosion = param8;
         this.qualityCollectables = param9;
         this.qualityEffect = param10;
      }
      
      public static function get instance() : QualitySettingsRequest
      {
         return _instance || (_instance = new QualitySettingsRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 264;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 19;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.qualityAttack = param1.readShort();
         this.qualityAttack = 65535 & ((65535 & this.qualityAttack) << 3 | (65535 & this.qualityAttack) >>> 13);
         this.qualityAttack = this.qualityAttack > 32767 ? int(this.qualityAttack - 65536) : this.qualityAttack;
         this.qualityBackground = param1.readShort();
         this.qualityBackground = 65535 & ((65535 & this.qualityBackground) >>> 9 | (65535 & this.qualityBackground) << 7);
         this.qualityBackground = this.qualityBackground > 32767 ? int(this.qualityBackground - 65536) : this.qualityBackground;
         this.qualityPresetting = param1.readShort();
         this.qualityPresetting = 65535 & ((65535 & this.qualityPresetting) << 11 | (65535 & this.qualityPresetting) >>> 5);
         this.qualityPresetting = this.qualityPresetting > 32767 ? int(this.qualityPresetting - 65536) : this.qualityPresetting;
         this.qualityCustomized = param1.readBoolean();
         this.qualityPOIzone = param1.readShort();
         this.qualityPOIzone = 65535 & ((65535 & this.qualityPOIzone) << 1 | (65535 & this.qualityPOIzone) >>> 15);
         this.qualityPOIzone = this.qualityPOIzone > 32767 ? int(this.qualityPOIzone - 65536) : this.qualityPOIzone;
         this.qualityShip = param1.readShort();
         this.qualityShip = 65535 & ((65535 & this.qualityShip) << 14 | (65535 & this.qualityShip) >>> 2);
         this.qualityShip = this.qualityShip > 32767 ? int(this.qualityShip - 65536) : this.qualityShip;
         this.qualityEngine = param1.readShort();
         this.qualityEngine = 65535 & ((65535 & this.qualityEngine) << 10 | (65535 & this.qualityEngine) >>> 6);
         this.qualityEngine = this.qualityEngine > 32767 ? int(this.qualityEngine - 65536) : this.qualityEngine;
         this.qualityExplosion = param1.readShort();
         this.qualityExplosion = 65535 & ((65535 & this.qualityExplosion) >>> 8 | (65535 & this.qualityExplosion) << 8);
         this.qualityExplosion = this.qualityExplosion > 32767 ? int(this.qualityExplosion - 65536) : this.qualityExplosion;
         this.qualityCollectables = param1.readShort();
         this.qualityCollectables = 65535 & ((65535 & this.qualityCollectables) << 4 | (65535 & this.qualityCollectables) >>> 12);
         this.qualityCollectables = this.qualityCollectables > 32767 ? int(this.qualityCollectables - 65536) : this.qualityCollectables;
         this.qualityEffect = param1.readShort();
         this.qualityEffect = 65535 & ((65535 & this.qualityEffect) >>> 5 | (65535 & this.qualityEffect) << 11);
         this.qualityEffect = this.qualityEffect > 32767 ? int(this.qualityEffect - 65536) : this.qualityEffect;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(264);
         param1.writeShort(65535 & ((65535 & this.qualityAttack) >>> 3 | (65535 & this.qualityAttack) << 13));
         param1.writeShort(65535 & ((65535 & this.qualityBackground) << 9 | (65535 & this.qualityBackground) >>> 7));
         param1.writeShort(65535 & ((65535 & this.qualityPresetting) >>> 11 | (65535 & this.qualityPresetting) << 5));
         param1.writeBoolean(this.qualityCustomized);
         param1.writeShort(65535 & ((65535 & this.qualityPOIzone) >>> 1 | (65535 & this.qualityPOIzone) << 15));
         param1.writeShort(65535 & ((65535 & this.qualityShip) >>> 14 | (65535 & this.qualityShip) << 2));
         param1.writeShort(65535 & ((65535 & this.qualityEngine) >>> 10 | (65535 & this.qualityEngine) << 6));
         param1.writeShort(65535 & ((65535 & this.qualityExplosion) << 8 | (65535 & this.qualityExplosion) >>> 8));
         param1.writeShort(65535 & ((65535 & this.qualityCollectables) >>> 4 | (65535 & this.qualityCollectables) << 12));
         param1.writeShort(65535 & ((65535 & this.qualityEffect) << 5 | (65535 & this.qualityEffect) >>> 11));
      }
   }
}

