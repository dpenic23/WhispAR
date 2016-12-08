import UIKit

class ViewController: UIViewController {
    var superpowered:Superpowered!
    var displayLink:CADisplayLink!
    var layers:[CALayer]!

    override func viewDidLoad() {
        super.viewDidLoad()

        // Setup 8 layers for frequency bars.
        let color:CGColor = UIColor(red: 0, green: 0.6, blue: 0.8, alpha: 1).cgColor
        layers = [CALayer(), CALayer(), CALayer(), CALayer(), CALayer(), CALayer(), CALayer(), CALayer()]
        for n in 0...7 {
            layers[n].backgroundColor = color
            layers[n].frame = CGRect.zero
            self.view.layer.addSublayer(layers[n])
        }

        superpowered = Superpowered()

        // A display link calls us on every frame (60 fps).
        displayLink = CADisplayLink(target: self, selector: #selector(ViewController.onDisplayLink))
        displayLink.frameInterval = 1
        displayLink.add(to: RunLoop.current, forMode: RunLoopMode.commonModes)
    }

    func onDisplayLink() {
        // Get the frequency values.
        let frequencies = UnsafeMutablePointer<Float>.allocate(capacity: 8)
        superpowered.getFrequencies(frequencies)

        // Wrapping the UI changes in a CATransaction block like this prevents animation/smoothing.
        CATransaction.begin()
        CATransaction.setAnimationDuration(0)
        CATransaction.setDisableActions(true)

        // Set the dimension of every frequency bar.
        let originY:CGFloat = self.view.frame.size.height - 20
        let width:CGFloat = (self.view.frame.size.width - 47) / 8
        var frame:CGRect = CGRect(x: 20, y: 0, width: width, height: 0)
        for n in 0...7 {
            frame.size.height = CGFloat(frequencies[n]) * 4000
            frame.origin.y = originY - frame.size.height
            layers[n].frame = frame
            frame.origin.x += width + 1
        }

        CATransaction.commit()
        frequencies.deallocate(capacity: 8)
    }
}

