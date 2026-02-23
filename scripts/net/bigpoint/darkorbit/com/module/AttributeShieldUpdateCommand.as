package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AttributeShieldUpdateCommand implements IModule
   {
      private static var _instance:AttributeShieldUpdateCommand;
      
      public var shieldNow:int = 0;
      
      public var shieldMax:int = 0;
      
      public function AttributeShieldUpdateCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.shieldNow = param1;
         this.shieldMax = param2;
      }
      
      public static function get instance() : AttributeShieldUpdateCommand
      {
         return _instance || (_instance = new AttributeShieldUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 248;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.shieldNow = param1.readInt();
         this.shieldNow = this.shieldNow >>> 16 | this.shieldNow << 16;
         this.shieldMax = param1.readInt();
         this.shieldMax = this.shieldMax >>> 12 | this.shieldMax << 20;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(248);
         param1.writeInt(this.shieldNow << 16 | this.shieldNow >>> 16);
         param1.writeInt(this.shieldMax << 12 | this.shieldMax >>> 20);
      }
   }
}

