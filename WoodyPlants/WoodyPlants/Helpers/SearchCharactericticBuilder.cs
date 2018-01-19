using PortableApp;
using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace WoodyPlants.Helpers
{
    class SearchCharactericticBuilder
    {
        public ObservableCollection<WoodySearch> searchCriteriaDB;
        public ObservableCollection<SearchCharacteristicIcon> searchCriteria;

        /*
        private void LeafShapeSearch()
        {
            Label leafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafShapeLabel);

            WrapLayout leafShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon narrowLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Narrow");
            leafShapeLayout.Children.Add(narrowLeafShape);

            SearchCharacteristicIcon deltoidLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Deltoid");
            leafShapeLayout.Children.Add(deltoidLeafShape);

            SearchCharacteristicIcon orbicularLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Orbicular");
            leafShapeLayout.Children.Add(orbicularLeafShape);

            SearchCharacteristicIcon oblanceolateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Oblanceolate");
            leafShapeLayout.Children.Add(oblanceolateLeafShape);

            SearchCharacteristicIcon palmatelyLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Palmately");
            leafShapeLayout.Children.Add(palmatelyLeafShape);

            SearchCharacteristicIcon lobedLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Lobed");
            leafShapeLayout.Children.Add(lobedLeafShape);

            SearchCharacteristicIcon pinnateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Pinnate");
            leafShapeLayout.Children.Add(pinnateLeafShape);

            searchFilters.Children.Add(leafShapeLayout);
        }

        private void LeafArrangementSearch()
        {
            // Add Type of Plant
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            WrapLayout leafArrangementLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon alternateLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Alternate");
            leafArrangementLayout.Children.Add(alternateLeafArrangement);

            SearchCharacteristicIcon oppositeLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Opposite");
            leafArrangementLayout.Children.Add(oppositeLeafArrangement);

            SearchCharacteristicIcon whorledLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Whorled");
            leafArrangementLayout.Children.Add(whorledLeafArrangement);

            SearchCharacteristicIcon basalLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Basal");
            leafArrangementLayout.Children.Add(basalLeafArrangement);

            searchFilters.Children.Add(leafArrangementLayout);
        }

        private void TwigTextureSearch()
        {
            // Add Type of Plant
            Label twigTextureLabel = new Label { Text = "Twig Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(twigTextureLabel);

            WrapLayout twigTextureLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon hairyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Hairy");
            twigTextureLayout.Children.Add(hairyTwigTexture);

            SearchCharacteristicIcon smoothTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Smooth");
            twigTextureLayout.Children.Add(smoothTwigTexture);

            SearchCharacteristicIcon roughTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Rough");
            twigTextureLayout.Children.Add(roughTwigTexture);

            SearchCharacteristicIcon peelingTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Peeling");
            twigTextureLayout.Children.Add(peelingTwigTexture);

            SearchCharacteristicIcon thornyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Thorny");
            twigTextureLayout.Children.Add(thornyTwigTexture);

            SearchCharacteristicIcon stickyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Sticky");
            twigTextureLayout.Children.Add(stickyTwigTexture);

            searchFilters.Children.Add(twigTextureLayout);
        }

        private void BarkTextureSearch()
        {
            // Add Type of Plant
            Label barkTextureLabel = new Label { Text = "Bark Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(barkTextureLabel);

            WrapLayout barkTextureLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon barkTextureSmooth = searchCriteria.First(x => x.Characteristic == "BarkTexture-Smooth");
            barkTextureLayout.Children.Add(barkTextureSmooth);

            SearchCharacteristicIcon barkTextureBumpy = searchCriteria.First(x => x.Characteristic == "BarkTexture-Bumpy");
            barkTextureLayout.Children.Add(barkTextureBumpy);

            SearchCharacteristicIcon barkTexturePeeling = searchCriteria.First(x => x.Characteristic == "BarkTexture-Peeling");
            barkTextureLayout.Children.Add(barkTexturePeeling);

            SearchCharacteristicIcon barkTextureFurrowed = searchCriteria.First(x => x.Characteristic == "BarkTexture-Furrowed");
            barkTextureLayout.Children.Add(barkTextureFurrowed);

            searchFilters.Children.Add(barkTextureLayout);
        }

        private void FlowerCluserSearch()
        {
            // Add Type of Plant
            Label flowerClusterLabel = new Label { Text = "Flower Cluster:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerClusterLabel);

            WrapLayout flowerClusterLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerClusterDense = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Dense");
            flowerClusterLayout.Children.Add(flowerClusterDense);

            SearchCharacteristicIcon flowerClusterLoose = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Loose");
            flowerClusterLayout.Children.Add(flowerClusterLoose);

            SearchCharacteristicIcon flowerClusterSolitary = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Solitary");
            flowerClusterLayout.Children.Add(flowerClusterSolitary);

            SearchCharacteristicIcon flowerClusterCatkin = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Catkin");
            flowerClusterLayout.Children.Add(flowerClusterCatkin);

            searchFilters.Children.Add(flowerClusterLayout);
        }

        private void FlowerShapeSearch()
        {
            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);

            WrapLayout flowerShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerShapeInconspicuous = searchCriteria.First(x => x.Characteristic == "FlowerShape-Inconspicuous");
            flowerShapeLayout.Children.Add(flowerShapeInconspicuous);

            SearchCharacteristicIcon flowerShapeRound = searchCriteria.First(x => x.Characteristic == "FlowerShape-Round");
            flowerShapeLayout.Children.Add(flowerShapeRound);

            SearchCharacteristicIcon flowerShapeBellShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-BellShaped");
            flowerShapeLayout.Children.Add(flowerShapeBellShaped);

            SearchCharacteristicIcon flowerShapeCupShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-CupShaped");
            flowerShapeLayout.Children.Add(flowerShapeCupShaped);

            SearchCharacteristicIcon flowerShapeStarShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-StarShaped");
            flowerShapeLayout.Children.Add(flowerShapeStarShaped);

            SearchCharacteristicIcon flowerShapeOther = searchCriteria.First(x => x.Characteristic == "FlowerShape-Other");
            flowerShapeLayout.Children.Add(flowerShapeOther);

            searchFilters.Children.Add(flowerShapeLayout);
        }

        private void FruitTypeSearch()
        {

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitTypeLabel);

            WrapLayout fruitTypeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon fruitTypeDrySeed = searchCriteria.First(x => x.Characteristic == "FruitType-DrySeed");
            fruitTypeLayout.Children.Add(fruitTypeDrySeed);

            SearchCharacteristicIcon fruitTypeAcorn = searchCriteria.First(x => x.Characteristic == "FruitType-Acorn");
            fruitTypeLayout.Children.Add(fruitTypeAcorn);

            SearchCharacteristicIcon fruitTypeFleshy = searchCriteria.First(x => x.Characteristic == "FruitType-Fleshy");
            fruitTypeLayout.Children.Add(fruitTypeFleshy);

            SearchCharacteristicIcon fruitTypeCone = searchCriteria.First(x => x.Characteristic == "FruitType-Cone");
            fruitTypeLayout.Children.Add(fruitTypeCone);

            SearchCharacteristicIcon fruitTypeCapsule = searchCriteria.First(x => x.Characteristic == "FruitType-Capsule");
            fruitTypeLayout.Children.Add(fruitTypeCapsule);

            SearchCharacteristicIcon fruitTypeSamara = searchCriteria.First(x => x.Characteristic == "FruitType-Samara");
            fruitTypeLayout.Children.Add(fruitTypeSamara);

            searchFilters.Children.Add(fruitTypeLayout);

        }

        private void FruitColorSearch()
        {
            // Add Type of Plant
            Label fruitColorLabel = new Label { Text = "Fruit Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitColorLabel);

            WrapLayout fruitColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Yellow");
            fruitColorLayout.Children.Add(yellowFruitColor);

            SearchCharacteristicIcon blueFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Blue");
            fruitColorLayout.Children.Add(blueFruitColor);

            SearchCharacteristicIcon redFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Red");
            fruitColorLayout.Children.Add(redFruitColor);

            SearchCharacteristicIcon brownFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Brown");
            fruitColorLayout.Children.Add(brownFruitColor);

            SearchCharacteristicIcon whiteFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-White");
            fruitColorLayout.Children.Add(whiteFruitColor);

            SearchCharacteristicIcon greenFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Green");
            fruitColorLayout.Children.Add(greenFruitColor);

            SearchCharacteristicIcon orangeFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Orange");
            fruitColorLayout.Children.Add(orangeFruitColor);

            SearchCharacteristicIcon blackFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Black");
            fruitColorLayout.Children.Add(blackFruitColor);

            SearchCharacteristicIcon purpleFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Purple");
            fruitColorLayout.Children.Add(purpleFruitColor);

            SearchCharacteristicIcon grayFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Gray");
            fruitColorLayout.Children.Add(grayFruitColor);

            searchFilters.Children.Add(fruitColorLayout);
        }

        private void FlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            flowerColorLayout.Children.Add(yellowFlowerColor);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            flowerColorLayout.Children.Add(blueFlowerColor);

            SearchCharacteristicIcon redFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Red");
            flowerColorLayout.Children.Add(redFlowerColor);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            flowerColorLayout.Children.Add(brownFlowerColor);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            flowerColorLayout.Children.Add(whiteFlowerColor);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
            flowerColorLayout.Children.Add(greenFlowerColor);

            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
            flowerColorLayout.Children.Add(orangeFlowerColor);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            flowerColorLayout.Children.Add(pinkFlowerColor);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            flowerColorLayout.Children.Add(purpleFlowerColor);

            searchFilters.Children.Add(flowerColorLayout);
        }

        private void LeafTypeSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Needle Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            flowerColorLayout.Children.Add(yellowFlowerColor);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            flowerColorLayout.Children.Add(blueFlowerColor);

            SearchCharacteristicIcon redFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Red");
            flowerColorLayout.Children.Add(redFlowerColor);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            flowerColorLayout.Children.Add(brownFlowerColor);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            flowerColorLayout.Children.Add(whiteFlowerColor);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
            flowerColorLayout.Children.Add(greenFlowerColor);

            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
            flowerColorLayout.Children.Add(orangeFlowerColor);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            flowerColorLayout.Children.Add(pinkFlowerColor);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            flowerColorLayout.Children.Add(purpleFlowerColor);

            searchFilters.Children.Add(flowerColorLayout);
        }*/
    }
}
