package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MapAssetIconRemoveHighlightCommand implements IModule
   {
      private static var _instance:MapAssetIconRemoveHighlightCommand;
      
      public var mapAssetId:int = 0;
      
      public function MapAssetIconRemoveHighlightCommand(param1:int = 0)
      {
         super();
         this.mapAssetId = param1;
      }
      
      public static function get instance() : MapAssetIconRemoveHighlightCommand
      {
         return _instance || (_instance = new MapAssetIconRemoveHighlightCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 24582;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.mapAssetId = param1.readInt();
         this.mapAssetId = this.mapAssetId << 9 | this.mapAssetId >>> 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(24582);
         param1.writeInt(this.mapAssetId >>> 9 | this.mapAssetId << 23);
      }
   }
}

