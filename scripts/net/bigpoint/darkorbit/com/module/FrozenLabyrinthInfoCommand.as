package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class FrozenLabyrinthInfoCommand implements IModule
   {
      private static var _instance:FrozenLabyrinthInfoCommand;
      
      public var labyrinthKeys:int = 0;
      
      public var timeInfo:FrozenLabyrinthTimeInfoCommand;
      
      public var rankInfo:FrozenLabyrinthRankInfoCommand;
      
      public function FrozenLabyrinthInfoCommand(param1:int = 0, param2:FrozenLabyrinthTimeInfoCommand = null, param3:FrozenLabyrinthRankInfoCommand = null)
      {
         super();
         this.labyrinthKeys = param1;
         if(param2 == null)
         {
            this.timeInfo = new FrozenLabyrinthTimeInfoCommand();
         }
         else
         {
            this.timeInfo = param2;
         }
         if(param3 == null)
         {
            this.rankInfo = new FrozenLabyrinthRankInfoCommand();
         }
         else
         {
            this.rankInfo = param3;
         }
      }
      
      public static function get instance() : FrozenLabyrinthInfoCommand
      {
         return _instance || (_instance = new FrozenLabyrinthInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -10407;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.labyrinthKeys = param1.readInt();
         this.labyrinthKeys = this.labyrinthKeys >>> 1 | this.labyrinthKeys << 31;
         this.timeInfo = FrozenLabyrinthTimeInfoCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.timeInfo)
         {
            this.timeInfo.read(param1);
         }
         this.rankInfo = FrozenLabyrinthRankInfoCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.rankInfo)
         {
            this.rankInfo.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-10407);
         param1.writeInt(this.labyrinthKeys << 1 | this.labyrinthKeys >>> 31);
         if(null != this.timeInfo)
         {
            this.timeInfo.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.rankInfo)
         {
            this.rankInfo.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

