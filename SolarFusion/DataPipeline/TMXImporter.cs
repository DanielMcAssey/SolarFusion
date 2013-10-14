using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace DataPipeline
{
    [ContentImporter(".tmx", DisplayName = "TMX Importer", DefaultProcessor = "TilemapProcessor")]
    public class TMXImporter : ContentImporter<PipelineTilemapContent>
    {
        string directory;
        List<string> ImagePaths = new List<string>();
        List<LevelTile> Tiles = new List<LevelTile>();

        public override PipelineTilemapContent Import(string filename, ContentImporterContext context)
        {
            directory = Path.GetDirectoryName(filename);

            PipelineTilemapContent output = new PipelineTilemapContent();
            output.tmName = Path.GetFileNameWithoutExtension(filename);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(filename, settings);

            List<PipelineTilemapData> layers = new List<PipelineTilemapData>();
            List<PipelineGameEntity> entities = new List<PipelineGameEntity>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.DocumentType:
                        if (reader.Name != "map")
                            throw new FormatException("Cannot parse map");
                        break;

                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "map":
                                output.tmWidth = int.Parse(reader.GetAttribute("width"));
                                output.tmHeight = int.Parse(reader.GetAttribute("height"));
                                output.tmTileWidth = int.Parse(reader.GetAttribute("tilewidth"));
                                output.tmTileHeight = int.Parse(reader.GetAttribute("tileheight"));
                                break;
                            case "properties":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    output.Properties = LoadProperties(st);
                                }
                                break;
                            case "tileset":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    LoadTileset(st, output.tmTileWidth, output.tmTileHeight);
                                }
                                break;
                            case "layer":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    layers.Add(LoadLayer(st));
                                }
                                break;
                            case "objectgroup":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    entities.Add(LoadObjectGroup(st, output.tmTileWidth, output.tmTileHeight));
                                }
                                break;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        break;

                    case XmlNodeType.Whitespace:
                        break;
                }
            }

            output.tmImagePaths = ImagePaths.ToArray();
            output.tmTileCount = Tiles.Count;
            output.tmTiles = Tiles.ToArray();
            output.tmLayerCount = layers.Count;
            output.tmLayers = layers.ToArray();
            output.tmGameEntityGroupCount = entities.Count;
            output.tmGameEntityGroups = entities.ToArray();

            return output;
        }

        protected Dictionary<string, string> LoadProperties(XmlReader reader)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "property")
                {
                    string key = reader.GetAttribute("name");
                    string value = reader.GetAttribute("value");
                    output[key] = value;
                }
            }

            return output;
        }

        void LoadTileset(XmlReader reader, int tileWidth, int tileHeight)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "image":
                            string imagePath = Path.Combine(directory, reader.GetAttribute("source"));
                            ImagePaths.Add(imagePath);

                            int width = int.Parse(reader.GetAttribute("width"));
                            int height = int.Parse(reader.GetAttribute("height"));

                            for (int y = 0; y < height / tileHeight; y++)
                            {
                                for (int x = 0; x < width / tileWidth; x++)
                                {
                                    LevelTile tile = new LevelTile();
                                    tile.textureID = ImagePaths.Count - 1;

                                    tile.source.X = x * tileWidth;
                                    tile.source.Y = y * tileHeight;
                                    tile.source.Width = tileWidth;
                                    tile.source.Height = tileHeight;

                                    Tiles.Add(tile);
                                }
                            }
                            break;
                    }
                }
            }
        }

        LevelTileData[] LoadTileData(XmlReader reader, int width, int height)
        {
            const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
            const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
            const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

            List<LevelTileData> TileData = new List<LevelTileData>(width * height);

            if (reader.GetAttribute("compression") != null)
            {
                throw new NotImplementedException("Cannot process compressed layer.");
            }

            switch (reader.GetAttribute("encoding"))
            {
                case "base64":
                    byte[] data = new byte[width * height * 4];
                    reader.ReadElementContentAsBase64(data, 0, width * height * 4);

                    int tileIndex = 0;
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            LevelTileData Tile = new LevelTileData();

                            uint globalTileID = (uint)(data[tileIndex] | data[tileIndex + 1] << 8 | data[tileIndex + 2] << 16 | data[tileIndex + 3] << 24);
                            tileIndex += 4;

                            Tile.SpriteEffects = SpriteEffects.None;
                            if ((globalTileID & FLIPPED_HORIZONTALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally;
                            if ((globalTileID & FLIPPED_VERTICALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipVertically;
                            if ((globalTileID & FLIPPED_DIAGONALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                            globalTileID &= ~(FLIPPED_VERTICALLY_FLAG | FLIPPED_HORIZONTALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);
                            Tile.TileID = globalTileID;
                            TileData.Add(Tile);
                        }
                    }
                    break;
                case "csv":
                    string csvData = reader.ReadElementContentAsString();
                    string[] splitData = csvData.Split(',');

                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            LevelTileData Tile = new LevelTileData();
                            uint globalTileID = UInt32.Parse(splitData[i * width + j]);

                            Tile.SpriteEffects = SpriteEffects.None;
                            if ((globalTileID & FLIPPED_HORIZONTALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally;
                            if ((globalTileID & FLIPPED_VERTICALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipVertically;
                            if ((globalTileID & FLIPPED_DIAGONALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                            globalTileID &= ~(FLIPPED_VERTICALLY_FLAG | FLIPPED_HORIZONTALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);
                            Tile.TileID = globalTileID;
                            TileData.Add(Tile);
                        }
                    } break;
                default:
                    throw new NotImplementedException("Unknown encoding in layer data");
            }

            return TileData.ToArray();
        }

        PipelineTilemapData LoadLayer(XmlReader reader)
        {
            PipelineTilemapData output = new PipelineTilemapData();
            output.Properties = new Dictionary<string, string>();

            string name = reader.GetAttribute("name");
            int width = int.Parse(reader.GetAttribute("width"));
            int height = int.Parse(reader.GetAttribute("height"));

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "data":
                            using (XmlReader st = reader.ReadSubtree())
                            {
                                st.Read();
                                output.TileData = LoadTileData(st, width, height);
                            }
                            break;
                        case "properties":
                            using (XmlReader st = reader.ReadSubtree())
                            {
                                st.Read();
                                output.Properties = LoadProperties(st);
                            }
                            break;
                    }
                }
            }

            return output;
        }

        PipelineGameEntity LoadObjectGroup(XmlReader reader, int tileWidth, int tileHeight)
        {
            PipelineGameEntity output = new PipelineGameEntity();
            output.Properties = new Dictionary<string, string>();

            List<GameEntity> gameObjects = new List<GameEntity>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "object":
                            GameEntity go = new GameEntity();
                            go.entCategory = reader.GetAttribute("name");
                            go.entType = reader.GetAttribute("type");

                            go.entPosition.X = int.Parse(reader.GetAttribute("x"));
                            go.entPosition.Y = int.Parse(reader.GetAttribute("y"));

                            if (reader.GetAttribute("width") != null)
                            {
                                go.entPosition.Width = int.Parse(reader.GetAttribute("width"));
                            }
                            else
                            {
                                go.entPosition.Width = tileWidth;
                            }
                            if (reader.GetAttribute("height") != null)
                            {
                                go.entPosition.Height = int.Parse(reader.GetAttribute("height"));
                            }
                            else
                            {
                                go.entPosition.Height = tileHeight;
                            }
                            gameObjects.Add(go);
                            break;
                        case "properties":
                            using (XmlReader st = reader.ReadSubtree())
                            {
                                st.Read();
                                output.Properties = LoadProperties(st);
                                if (output.Properties.ContainsKey("ScrollingSpeed"))
                                    output.ScrollSpeed = float.Parse(output.Properties["ScrollingSpeed"]);
                                if (output.Properties.ContainsKey("ScrollOffset"))
                                    output.ScrollOffset = float.Parse(output.Properties["ScrollOffset"]);
                            }
                            break;
                    }
                }
            }

            output.GameEntityData = new GameEntity[gameObjects.Count];
            output.GameEntityData = gameObjects.ToArray();

            return output;
        }
    }
}
