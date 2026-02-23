package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderSetupGateCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderSetupGateCommand;
      
      public static const ERRORCODE_NONE:int = 0;
      
      public static const ERRORCODE_UNCLASSIFIED_ERROR:int = 1;
      
      public static const ERRORCODE__MAX:int = 2;
      
      public var livesLeft:int = 0;
      
      public var errorCode:int = 0;
      
      public var id:int = 0;
      
      public var lifePrice:int = 0;
      
      public function GalaxyGateBuilderSetupGateCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.livesLeft = param2;
         this.errorCode = param4;
         this.id = param1;
         this.lifePrice = param3;
      }
      
      public static function get instance() : GalaxyGateBuilderSetupGateCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderSetupGateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 28963;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 9;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.livesLeft = param1.readInt();
         this.livesLeft = this.livesLeft << 15 | this.livesLeft >>> 17;
         this.errorCode = param1.readShort();
         this.id = param1.readByte();
         this.id = 255 & ((255 & this.id) << 0 | (255 & this.id) >>> 8);
         this.id = this.id > 127 ? int(this.id - 256) : this.id;
         this.lifePrice = param1.readInt();
         this.lifePrice = this.lifePrice << 15 | this.lifePrice >>> 17;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(28963);
         param1.writeInt(this.livesLeft >>> 15 | this.livesLeft << 17);
         param1.writeShort(this.errorCode);
         param1.writeByte(255 & ((255 & this.id) >>> 0 | (255 & this.id) << 8));
         param1.writeInt(this.lifePrice >>> 15 | this.lifePrice << 17);
      }
   }
}

