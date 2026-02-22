package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PetHeroActivationCommand implements IModule
   {
      private static var _instance:PetHeroActivationCommand;
      
      public var ownerId:int = 0;
      
      public var petId:int = 0;
      
      public var shipType:int = 0;
      
      public var expansionStage:int = 0;
      
      public var petName:String = "";
      
      public var factionId:int = 0;
      
      public var clanId:int = 0;
      
      public var petLevel:int = 0;
      
      public var clanTag:String = "";
      
      public var x:int = 0;
      
      public var y:int = 0;
      
      public var petSpeed:int = 0;
      
      public var minimapStrategyModule:MinimapStrategyModule;
      
      public function PetHeroActivationCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:String = "", param6:int = 0, param7:int = 0, param8:int = 0, param9:String = "", param10:int = 0, param11:int = 0, param12:int = 0, param13:MinimapStrategyModule = null)
      {
         super();
         this.ownerId = param1;
         this.petId = param2;
         this.shipType = param3;
         this.expansionStage = param4;
         this.petName = param5;
         this.factionId = param6;
         this.clanId = param7;
         this.petLevel = param8;
         this.clanTag = param9;
         this.x = param10;
         this.y = param11;
         this.petSpeed = param12;
         if(param13 == null)
         {
            this.minimapStrategyModule = new MinimapStrategyModule();
         }
         else
         {
            this.minimapStrategyModule = param13;
         }
      }
      
      public static function get instance() : PetHeroActivationCommand
      {
         return _instance || (_instance = new PetHeroActivationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 155;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 36;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.ownerId = param1.readInt();
         this.ownerId = this.ownerId >>> 14 | this.ownerId << 18;
         this.petId = param1.readInt();
         this.petId = this.petId << 5 | this.petId >>> 27;
         this.shipType = param1.readShort();
         this.shipType = 65535 & ((65535 & this.shipType) << 1 | (65535 & this.shipType) >>> 15);
         this.shipType = this.shipType > 32767 ? int(this.shipType - 65536) : this.shipType;
         this.expansionStage = param1.readShort();
         this.expansionStage = 65535 & ((65535 & this.expansionStage) >>> 11 | (65535 & this.expansionStage) << 5);
         this.expansionStage = this.expansionStage > 32767 ? int(this.expansionStage - 65536) : this.expansionStage;
         this.petName = param1.readUTF();
         this.factionId = param1.readShort();
         this.factionId = 65535 & ((65535 & this.factionId) << 1 | (65535 & this.factionId) >>> 15);
         this.factionId = this.factionId > 32767 ? int(this.factionId - 65536) : this.factionId;
         this.clanId = param1.readInt();
         this.clanId = this.clanId >>> 12 | this.clanId << 20;
         this.petLevel = param1.readShort();
         this.petLevel = 65535 & ((65535 & this.petLevel) >>> 7 | (65535 & this.petLevel) << 9);
         this.petLevel = this.petLevel > 32767 ? int(this.petLevel - 65536) : this.petLevel;
         this.clanTag = param1.readUTF();
         this.x = param1.readInt();
         this.x = this.x << 12 | this.x >>> 20;
         this.y = param1.readInt();
         this.y = this.y >>> 11 | this.y << 21;
         this.petSpeed = param1.readInt();
         this.petSpeed = this.petSpeed >>> 9 | this.petSpeed << 23;
         this.minimapStrategyModule = MinimapStrategyModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.minimapStrategyModule)
         {
            this.minimapStrategyModule.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(155);
         param1.writeInt(this.ownerId << 14 | this.ownerId >>> 18);
         param1.writeInt(this.petId >>> 5 | this.petId << 27);
         param1.writeShort(65535 & ((65535 & this.shipType) >>> 1 | (65535 & this.shipType) << 15));
         param1.writeShort(65535 & ((65535 & this.expansionStage) << 11 | (65535 & this.expansionStage) >>> 5));
         param1.writeUTF(this.petName);
         param1.writeShort(65535 & ((65535 & this.factionId) >>> 1 | (65535 & this.factionId) << 15));
         param1.writeInt(this.clanId << 12 | this.clanId >>> 20);
         param1.writeShort(65535 & ((65535 & this.petLevel) << 7 | (65535 & this.petLevel) >>> 9));
         param1.writeUTF(this.clanTag);
         param1.writeInt(this.x >>> 12 | this.x << 20);
         param1.writeInt(this.y << 11 | this.y >>> 21);
         param1.writeInt(this.petSpeed << 9 | this.petSpeed >>> 23);
         if(null != this.minimapStrategyModule)
         {
            this.minimapStrategyModule.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

