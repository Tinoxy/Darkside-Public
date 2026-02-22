package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class MapAddControlPOIZoneCommand extends MapAddPOICommand
   {
      private static var _instance:MapAddControlPOIZoneCommand;
      
      public var timeTillUnlock:Number = 0;
      
      public var progressOwner:FactionModule;
      
      public var captureThreshold:int = 0;
      
      public var captured:Boolean = false;
      
      public var captureProgress:int = 0;
      
      public function MapAddControlPOIZoneCommand(param1:String = "", param2:POITypeModule = null, param3:String = "", param4:POIDesignModule = null, param5:int = 0, param6:Vector.<int> = null, param7:Boolean = false, param8:Boolean = false, param9:FactionModule = null, param10:int = 0, param11:int = 0, param12:Number = 0, param13:Boolean = false)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
         this.timeTillUnlock = param12;
         if(param9 == null)
         {
            this.progressOwner = new FactionModule();
         }
         else
         {
            this.progressOwner = param9;
         }
         this.captureThreshold = param11;
         this.captured = param13;
         this.captureProgress = param10;
      }
      
      public static function get instance() : MapAddControlPOIZoneCommand
      {
         return _instance || (_instance = new MapAddControlPOIZoneCommand());
      }
      
      override public function getLibcomModuleId() : int
      {
         return -12016;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 17;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.timeTillUnlock = param1.readDouble();
         this.progressOwner = FactionModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.progressOwner)
         {
            this.progressOwner.read(param1);
         }
         this.captureThreshold = param1.readInt();
         this.captureThreshold = this.captureThreshold << 9 | this.captureThreshold >>> 23;
         this.captured = param1.readBoolean();
         this.captureProgress = param1.readInt();
         this.captureProgress = this.captureProgress << 5 | this.captureProgress >>> 27;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-12016);
         super.write(param1);
         param1.writeDouble(this.timeTillUnlock);
         if(null != this.progressOwner)
         {
            this.progressOwner.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.captureThreshold >>> 9 | this.captureThreshold << 23);
         param1.writeBoolean(this.captured);
         param1.writeInt(this.captureProgress >>> 5 | this.captureProgress << 27);
      }
   }
}

