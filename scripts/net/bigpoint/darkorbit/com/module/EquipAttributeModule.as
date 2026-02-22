package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EquipAttributeModule implements IModule
   {
      private static var _instance:EquipAttributeModule;
      
      public var AttributeValue:int = 0;
      
      public var maxAttributeValue:int = 0;
      
      public var AttributeID:String = "";
      
      public var upMin:int = 0;
      
      public var upMax:int = 0;
      
      public function EquipAttributeModule(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.AttributeValue = param2;
         this.maxAttributeValue = param3;
         this.AttributeID = param1;
         this.upMin = param4;
         this.upMax = param5;
      }
      
      public static function get instance() : EquipAttributeModule
      {
         return _instance || (_instance = new EquipAttributeModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -18831;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.AttributeValue = param1.readInt();
         this.AttributeValue = this.AttributeValue << 14 | this.AttributeValue >>> 18;
         this.maxAttributeValue = param1.readInt();
         this.maxAttributeValue = this.maxAttributeValue >>> 8 | this.maxAttributeValue << 24;
         this.AttributeID = param1.readUTF();
         this.upMin = param1.readInt();
         this.upMin = this.upMin << 2 | this.upMin >>> 30;
         this.upMax = param1.readInt();
         this.upMax = this.upMax << 11 | this.upMax >>> 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-18831);
         param1.writeInt(this.AttributeValue >>> 14 | this.AttributeValue << 18);
         param1.writeInt(this.maxAttributeValue << 8 | this.maxAttributeValue >>> 24);
         param1.writeUTF(this.AttributeID);
         param1.writeInt(this.upMin >>> 2 | this.upMin << 30);
         param1.writeInt(this.upMax >>> 11 | this.upMax << 21);
      }
   }
}

