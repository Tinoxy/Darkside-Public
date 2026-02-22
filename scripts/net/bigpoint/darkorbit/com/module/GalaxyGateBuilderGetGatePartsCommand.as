package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderGetGatePartsCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderGetGatePartsCommand;
      
      public static const ERRORCODE_NONE:int = 0;
      
      public static const ERRORCODE_UNCLASSIFIED_ERROR:int = 1;
      
      public static const ERRORCODE__MAX:int = 2;
      
      public var errorCode:int = 0;
      
      public var gateId:int = 0;
      
      public var gatePartIds:Vector.<int>;
      
      public function GalaxyGateBuilderGetGatePartsCommand(param1:int = 0, param2:Vector.<int> = null, param3:int = 0)
      {
         super();
         this.errorCode = param3;
         this.gateId = param1;
         if(param2 == null)
         {
            this.gatePartIds = new Vector.<int>();
         }
         else
         {
            this.gatePartIds = param2;
         }
      }
      
      public static function get instance() : GalaxyGateBuilderGetGatePartsCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderGetGatePartsCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -4472;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 5;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         this.errorCode = param1.readShort();
         this.gateId = param1.readByte();
         this.gateId = 255 & ((255 & this.gateId) >>> 6 | (255 & this.gateId) << 2);
         this.gateId = this.gateId > 127 ? int(this.gateId - 256) : this.gateId;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.gatePartIds.length > 0)
         {
            this.gatePartIds.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readShort());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readShort());
            _loc4_ = 65535 & ((65535 & _loc4_) >>> 14 | (65535 & _loc4_) << 2);
            _loc4_ = (65535 & ((65535 & _loc4_) >>> 14 | (65535 & _loc4_) << 2)) > 32767 ? int(_loc4_ - 65536) : _loc4_;
            this.gatePartIds.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(-4472);
         param1.writeShort(this.errorCode);
         param1.writeByte(255 & ((255 & this.gateId) << 6 | (255 & this.gateId) >>> 2));
         param1.writeShort(this.gatePartIds.length);
         for each(_loc2_ in this.gatePartIds)
         {
            param1.writeShort(65535 & ((65535 & _loc2_) << 14 | (65535 & _loc2_) >>> 2));
         }
      }
   }
}

