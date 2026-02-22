package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ShipBoostEventAttributeModule implements IModule
   {
      private static var _instance:ShipBoostEventAttributeModule;
      
      public var boostValue:Number = 0;
      
      public var attribute:String = "";
      
      public function ShipBoostEventAttributeModule(param1:String = "", param2:Number = 0)
      {
         super();
         this.boostValue = param2;
         this.attribute = param1;
      }
      
      public static function get instance() : ShipBoostEventAttributeModule
      {
         return _instance || (_instance = new ShipBoostEventAttributeModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -9828;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.boostValue = param1.readFloat();
         this.attribute = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-9828);
         param1.writeFloat(this.boostValue);
         param1.writeUTF(this.attribute);
      }
   }
}

