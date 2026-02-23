package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssetCreateCommand implements IModule
   {
      private static var _instance:AssetCreateCommand;
      
      public var showBubble:Boolean = false;
      
      public var assetId:int = 0;
      
      public var clanRelation:ClanRelationModule;
      
      public var modifier:Vector.<VisualModifierCommand>;
      
      public var designId:int = 0;
      
      public var invisible:Boolean = false;
      
      public var userName:String = "";
      
      public var visibleOnWarnRadar:Boolean = false;
      
      public var clanTag:String = "";
      
      public var factionId:int = 0;
      
      public var expansionStage:int = 0;
      
      public var clanId:int = 0;
      
      public var type:AssetTypeModule;
      
      public var detectedByWarnRadar:Boolean = false;
      
      public var posX:int = 0;
      
      public var posY:int = 0;
      
      public function AssetCreateCommand(param1:AssetTypeModule = null, param2:String = "", param3:int = 0, param4:String = "", param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0, param9:int = 0, param10:int = 0, param11:Boolean = false, param12:Boolean = false, param13:Boolean = false, param14:Boolean = false, param15:ClanRelationModule = null, param16:Vector.<VisualModifierCommand> = null)
      {
         super();
         this.showBubble = param14;
         this.assetId = param5;
         if(param15 == null)
         {
            this.clanRelation = new ClanRelationModule();
         }
         else
         {
            this.clanRelation = param15;
         }
         if(param16 == null)
         {
            this.modifier = new Vector.<VisualModifierCommand>();
         }
         else
         {
            this.modifier = param16;
         }
         this.designId = param6;
         this.invisible = param11;
         this.userName = param2;
         this.visibleOnWarnRadar = param12;
         this.clanTag = param4;
         this.factionId = param3;
         this.expansionStage = param7;
         this.clanId = param10;
         if(param1 == null)
         {
            this.type = new AssetTypeModule();
         }
         else
         {
            this.type = param1;
         }
         this.detectedByWarnRadar = param13;
         this.posX = param8;
         this.posY = param9;
      }
      
      public static function get instance() : AssetCreateCommand
      {
         return _instance || (_instance = new AssetCreateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 21957;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 40;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:VisualModifierCommand = null;
         this.showBubble = param1.readBoolean();
         this.assetId = param1.readInt();
         this.assetId = this.assetId >>> 1 | this.assetId << 31;
         this.clanRelation = ClanRelationModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.clanRelation)
         {
            this.clanRelation.read(param1);
         }
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.modifier.length > 0)
         {
            this.modifier.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = VisualModifierCommand(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.modifier.push(_loc4_);
            _loc2_++;
         }
         this.designId = param1.readInt();
         this.designId = this.designId << 5 | this.designId >>> 27;
         this.invisible = param1.readBoolean();
         this.userName = param1.readUTF();
         this.visibleOnWarnRadar = param1.readBoolean();
         this.clanTag = param1.readUTF();
         this.factionId = param1.readInt();
         this.factionId = this.factionId >>> 1 | this.factionId << 31;
         this.expansionStage = param1.readInt();
         this.expansionStage = this.expansionStage << 15 | this.expansionStage >>> 17;
         this.clanId = param1.readInt();
         this.clanId = this.clanId >>> 2 | this.clanId << 30;
         this.type = AssetTypeModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.type)
         {
            this.type.read(param1);
         }
         this.detectedByWarnRadar = param1.readBoolean();
         this.posX = param1.readInt();
         this.posX = this.posX << 15 | this.posX >>> 17;
         this.posY = param1.readInt();
         this.posY = this.posY << 11 | this.posY >>> 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:VisualModifierCommand = null;
         param1.writeShort(21957);
         param1.writeBoolean(this.showBubble);
         param1.writeInt(this.assetId << 1 | this.assetId >>> 31);
         if(null != this.clanRelation)
         {
            this.clanRelation.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeByte(this.modifier.length);
         for each(_loc2_ in this.modifier)
         {
            _loc2_.write(param1);
         }
         param1.writeInt(this.designId >>> 5 | this.designId << 27);
         param1.writeBoolean(this.invisible);
         param1.writeUTF(this.userName);
         param1.writeBoolean(this.visibleOnWarnRadar);
         param1.writeUTF(this.clanTag);
         param1.writeInt(this.factionId << 1 | this.factionId >>> 31);
         param1.writeInt(this.expansionStage >>> 15 | this.expansionStage << 17);
         param1.writeInt(this.clanId << 2 | this.clanId >>> 30);
         if(null != this.type)
         {
            this.type.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeBoolean(this.detectedByWarnRadar);
         param1.writeInt(this.posX >>> 15 | this.posX << 17);
         param1.writeInt(this.posY >>> 11 | this.posY << 21);
      }
   }
}

