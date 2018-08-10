using CalcolatoreXamarin.ViewModels;
using Org.Visiontech.Compute;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VisiontechCommons;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Shared.ViewModels
{
    public class LensPreviewModel : BaseViewModel
    {

        protected readonly ComputeSoapClient computeSoapClient = Container.ServiceProvider.GetService(typeof(ComputeSoapClient)) as ComputeSoapClient;

        public event EventHandler<Tuple<computeLensRequestDTO, computeLensResponseDTO>> ResponseComputed;
        public enum LensType
        {
            Digit,
            Punctual,
            Vario,
            Iflex,
            Iprof,
            Iprog,
            Magic,
            Office,
            Fantasy
        }
        public ObservableCollection<double> RefractionIndexes { get; }

        private bool computeButtonIsEnabled = false;
        public bool ComputeButtonIsEnabled
        {
            get { return computeButtonIsEnabled; }
            set { SetProperty(ref computeButtonIsEnabled, value); }
        }

        private bool farNearLayoutIsVisible = false;
        public bool FarNearLayoutIsVisible
        {
            get { return farNearLayoutIsVisible; }
            set { SetProperty(ref farNearLayoutIsVisible, value); }
        }

        private bool channelLayoutIsVisible = false;
        public bool ChannelLayoutIsVisible
        {
            get { return channelLayoutIsVisible; }
            set { SetProperty(ref channelLayoutIsVisible, value); }
        }

        private LensType? design;
        public LensType? Design
        {
            get { return design; }
            set {
                SetProperty(ref design, value);
                CheckComputeButtonIsEnabled();

                switch (value)
                {
                    case LensType.Digit:
                    case LensType.Vario:
                    case LensType.Iflex:
                        FarNearLayoutIsVisible = true;
                        ChannelLayoutIsVisible = true;
                        break;
                    default:
                        FarNearLayoutIsVisible = false;
                        ChannelLayoutIsVisible = false;
                        break;
                }
            }
        }

        private double? refractionIndex;
        public double? RefractionIndex
        {
            get { return refractionIndex; }
            set {
                SetProperty(ref refractionIndex, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private int farZone = 80;
        public int FarZone
        {
            get { return farZone; }
            set { SetProperty(ref farZone, value); }
        }

        private int nearZone = 20;
        public int NearZone
        {
            get { return nearZone; }
            set { SetProperty(ref nearZone, value); }
        }

        private int channel = 10;
        public int Channel
        {
            get { return channel; }
            set { SetProperty(ref channel, value); }
        }

        private double leftSphere = 0;
        public double LeftSphere
        {
            get { return leftSphere; }
            set
            {
                SetProperty(ref leftSphere, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double rightSphere = 0;
        public double RightSphere
        {
            get { return rightSphere; }
            set
            {
                SetProperty(ref rightSphere, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double leftCylinder = 0;
        public double LeftCylinder
        {
            get { return leftCylinder; }
            set
            {
                SetProperty(ref leftCylinder, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double rightCylinder = 0;
        public double RightCylinder
        {
            get { return rightCylinder; }
            set
            {
                SetProperty(ref rightCylinder, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private int leftCylinderAxis = 0;
        public int LeftCylinderAxis
        {
            get { return leftCylinderAxis; }
            set
            {
                SetProperty(ref leftCylinderAxis, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private int rightCylinderAxis = 0;
        public int RightCylinderAxis
        {
            get { return rightCylinderAxis; }
            set
            {
                SetProperty(ref rightCylinderAxis, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double leftAddiction = 2;
        public double LeftAddiction
        {
            get { return leftAddiction; }
            set
            {
                SetProperty(ref leftAddiction, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double rightAddiction = 2;
        public double RightAddiction
        {
            get { return rightAddiction; }
            set
            {
                SetProperty(ref rightAddiction, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double leftToolRealBase = 5;
        public double LeftToolRealBase
        {
            get { return leftToolRealBase; }
            set
            {
                SetProperty(ref leftToolRealBase, value);
                CheckComputeButtonIsEnabled();
            }
        }

        private double rightToolRealBase = 5;
        public double RightToolRealBase
        {
            get { return rightToolRealBase; }
            set
            {
                SetProperty(ref rightToolRealBase, value);
                CheckComputeButtonIsEnabled();
            }
        }
        public ObservableCollection<LensType> LensTypes { get; }
        public ICommand ComputeLensCommand { get; }

        public LensPreviewModel()
        {
            RefractionIndexes = new ObservableCollection<double>() { 1.498, 1.56, 1.61, 1.67, 1.74 };
            LensTypes = new ObservableCollection<LensType>(Enum.GetValues(typeof(LensType)).Cast<LensType>().ToList());
            ComputeLensCommand = new Command(ComputeLens);
        }
        private void CheckComputeButtonIsEnabled()
        {
            ComputeButtonIsEnabled = Design != null && RefractionIndex != null;
        }
        private async void ComputeLens()
        {

            IsBusy = true;

            computeLensRequestDTO request = null;

            switch (Design)
            {
                case LensType.Digit:
                    request = new computeDigitLensRequestDTO()
                    {
                        far = FarZone,
                        farSpecified = true,
                        near = NearZone,
                        nearSpecified = true,
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Punctual:
                    request = new computePunctualLensRequestDTO()
                    {
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Vario:
                    request = new computeVarioLensRequestDTO()
                    {
                        far = FarZone,
                        farSpecified = true,
                        near = NearZone,
                        nearSpecified = true,
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Iflex:
                    request = new computeIflexLensRequestDTO()
                    {
                        far = FarZone,
                        farSpecified = true,
                        near = NearZone,
                        nearSpecified = true,
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Iprof:
                    request = new computeIprofLensRequestDTO()
                    {
                        near = NearZone,
                        nearSpecified = true,
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Iprog:
                    request = new computeIprogLensRequestDTO()
                    {
                        near = NearZone,
                        nearSpecified = true,
                        channel = Channel,
                        channelSpecified = true
                    };
                    break;
                case LensType.Magic:
                    request = new computeMagicLensRequestDTO();
                    break;
                case LensType.Office:
                    request = new computeOfficeLensRequestDTO();
                    break;
                case LensType.Fantasy:
                    request = new computeFantasyLensRequestDTO();
                    break;
            }

            request.preview = false;
            request.previewSpecified = true;
            request.verticalPrismThinning = true;
            request.verticalPrismThinningSpecified = true;
            request.horizontalPrismThinning = false;
            request.horizontalPrismThinningSpecified = true;
            request.horizontalPrismThinningHeight = 0;
            request.horizontalPrismThinningHeightSpecified = true;
            request.prismThinningCompensation = false;
            request.prismThinningCompensationSpecified = true;
            request.maximumPrismThinningCompensation = 0;
            request.maximumPrismThinningCompensationSpecified = true;
            request.atoric = false;
            request.atoricSpecified = true;
            request.pantoscopicAngle = 9;
            request.pantoscopicAngleSpecified = true;
            request.wrappingAngle = 6;
            request.wrappingAngleSpecified = true;
            request.backVertexDistance = 13;
            request.backVertexDistanceSpecified = true;
            request.eyeDiameter = 26;
            request.eyeDiameterSpecified = true;
            request.readDistance = 330;
            request.readDistanceSpecified = true;

            request.left = new computeLensRequestSideDTO()
            {
                sphere = LeftSphere,
                sphereSpecified = true,
                cylinder = LeftCylinder,
                cylinderSpecified = true,
                cylinderAxis = LeftCylinderAxis,
                cylinderAxisSpecified = true,
                addiction = LeftAddiction,
                addictionSpecified = true,
                horizontalDiameter = 70,
                horizontalDiameterSpecified = true,
                verticalDiameter = 70,
                verticalDiameterSpecified = true,
                refractiveIndex = RefractionIndex.Value,
                refractiveIndexSpecified = true,
                toolRealBase = LeftToolRealBase,
                toolRealBaseSpecified = true,
                toolIndex = 1.498,
                toolIndexSpecified = true,
                prescriptedPrism = 0,
                prescriptedPrismSpecified = true,
                prescriptedPrismBase = 0,
                prescriptedPrismBaseSpecified = true,
                secondaryPrism = 0,
                secondaryPrismSpecified = true,
                secondaryPrismBase = 0,
                secondaryPrismBaseSpecified = true,
                minimalCentralThickness = 2,
                minimalCentralThicknessSpecified = true,
                minimalSideThickness = 2,
                minimalSideThicknessSpecified = true,
                horizontalDecentralization = 0,
                horizontalDecentralizationSpecified = true,
                verticalDecentralization = 0,
                verticalDecentralizationSpecified = true,
                monocularCentrationDistance = 32,
                monocularCentrationDistanceSpecified = true,
                inset = 2.2,
                insetSpecified = true
            };
            request.right = new computeLensRequestSideDTO()
            {
                sphere = RightSphere,
                sphereSpecified = true,
                cylinder = RightCylinder,
                cylinderSpecified = true,
                cylinderAxis = RightCylinderAxis,
                cylinderAxisSpecified = true,
                addiction = RightAddiction,
                addictionSpecified = true,
                horizontalDiameter = 70,
                horizontalDiameterSpecified = true,
                verticalDiameter = 70,
                verticalDiameterSpecified = true,
                refractiveIndex = RefractionIndex.Value,
                refractiveIndexSpecified = true,
                toolRealBase = RightToolRealBase,
                toolRealBaseSpecified = true,
                toolIndex = 1.498,
                toolIndexSpecified = true,
                prescriptedPrism = 0,
                prescriptedPrismSpecified = true,
                prescriptedPrismBase = 0,
                prescriptedPrismBaseSpecified = true,
                secondaryPrism = 0,
                secondaryPrismSpecified = true,
                secondaryPrismBase = 0,
                secondaryPrismBaseSpecified = true,
                minimalCentralThickness = 2,
                minimalCentralThicknessSpecified = true,
                minimalSideThickness = 2,
                minimalSideThicknessSpecified = true,
                horizontalDecentralization = 0,
                horizontalDecentralizationSpecified = true,
                verticalDecentralization = 0,
                verticalDecentralizationSpecified = true,
                monocularCentrationDistance = 32,
                monocularCentrationDistanceSpecified = true,
                inset = 2.2,
                insetSpecified = true
            };

            computeLensResponseDTO response = await Task.Run(() => computeSoapClient.computeLens(request)) as computeLensResponseDTO;

            if (!(response is null))
            {
                ResponseComputed.Invoke(this, Tuple.Create(request, response));
            }

            IsBusy = false;

        }

    }
}
