package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MapAddPOICommand implements IModule
   {
      private static var _instance:MapAddPOICommand;
      
      public static const SHAPETYPE_CIRCLE:int = 0;
      
      public static const SHAPETYPE_POLYGON:int = 1;
      
      public static const SHAPETYPE_RECTANGLE:int = 2;
      
      public static const SHAPETYPE__MAX:int = 3;
      
      public var poiId:String = "";
      
      public var design:POIDesignModule;
      
      public var inverted:Boolean = false;
      
      public var shape:int = 0;
      
      public var poiTypeSpecification:String = "";
      
      public var poiType:POITypeModule;
      
      public var shapeCoordinates:Vector.<int>;
      
      public var active:Boolean = false;
      
      public function MapAddPOICommand(param1:String = "", param2:POITypeModule = null, param3:String = "", param4:POIDesignModule = null, param5:int = 0, param6:Vector.<int> = null, param7:Boolean = false, param8:Boolean = false)
      {
         super();
         this.poiId = param1;
         if(param4 == null)
         {
            this.design = new POIDesignModule();
         }
         else
         {
            this.design = param4;
         }
         this.inverted = param7;
         this.shape = param5;
         this.poiTypeSpecification = param3;
         if(param2 == null)
         {
            this.poiType = new POITypeModule();
         }
         else
         {
            this.poiType = param2;
         }
         if(param6 == null)
         {
            this.shapeCoordinates = new Vector.<int>();
         }
         else
         {
            this.shapeCoordinates = param6;
         }
         this.active = param8;
      }
      
      public static function get instance() : MapAddPOICommand
      {
         return _instance || (_instance = new MapAddPOICommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 25558;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         this.poiId = param1.readUTF();
         this.design = POIDesignModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.design)
         {
            this.design.read(param1);
         }
         this.inverted = param1.readBoolean();
         this.shape = param1.readShort();
         this.poiTypeSpecification = param1.readUTF();
         this.poiType = POITypeModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.poiType)
         {
            this.poiType.read(param1);
         }
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.shapeCoordinates.length > 0)
         {
            this.shapeCoordinates.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readInt());
            _loc4_ = _loc4_ >>> 6 | _loc4_ << 26;
            this.shapeCoordinates.push(_loc4_);
            _loc2_++;
         }
         this.active = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(25558);
         param1.writeUTF(this.poiId);
         if(null != this.design)
         {
            this.design.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeBoolean(this.inverted);
         param1.writeShort(this.shape);
         param1.writeUTF(this.poiTypeSpecification);
         if(null != this.poiType)
         {
            this.poiType.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeByte(this.shapeCoordinates.length);
         for each(_loc2_ in this.shapeCoordinates)
         {
            param1.writeInt(_loc2_ << 6 | _loc2_ >>> 26);
         }
         param1.writeBoolean(this.active);
      }
   }
}

