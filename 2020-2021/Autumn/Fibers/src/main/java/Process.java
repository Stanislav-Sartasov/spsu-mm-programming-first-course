import java.util.ArrayList;
import java.util.Random;

import static java.lang.Thread.sleep;

public class Process {
	private static final int longPauseBoundary = 10000;
	private static final int shortPauseBoundary = 100;
	private static final int workBoundary = 1000;
	private static final int intervalsAmountBoundary = 10;
	private static final int priorityLevelsNumber = 10;
	private final ArrayList<Integer> workIntervals = new ArrayList<>();
	private final ArrayList<Integer> pauseIntervals = new ArrayList<>();
	private int priority;
	private int totalDuration;
	private int activeDuration;
	private volatile boolean isFinished = false;

	public boolean isFinished() {
		return isFinished;
	}

	public ArrayList<Integer> getWorkIntervals() {
		return workIntervals;
	}

	public int getPriority() {
		return priority;
	}

	public void setPriority(int priority) {
		this.priority = priority;
	}

	public int getTotalDuration() {
		return activeDuration + sumOfElements(pauseIntervals);
	}

	public int getActiveDuration() {
		return sumOfElements(workIntervals);
	}

	public Process() {
		Random random = new Random();
		int amount = random.nextInt(intervalsAmountBoundary);
		for (int i = 0; i < amount; i++) {
			workIntervals.add(random.nextInt(workBoundary));
			pauseIntervals.add(random.nextInt(
					random.nextDouble() > 0.9
							? longPauseBoundary
							: shortPauseBoundary));
		}
		priority = random.nextInt(priorityLevelsNumber);
	}

	public void run() throws InterruptedException {
		synchronized (this) {
			for (int i = 0; i < workIntervals.size(); i++) {
				sleep(workIntervals.get(i)); // work emulation
				long pauseBeginTime = System.currentTimeMillis();
				do {
					Thread.sleep(1);
					ProcessManager.processManagerSwitch(false);
				} while ((System.currentTimeMillis() - pauseBeginTime) < pauseIntervals.get(i)); // I/O emulation*/
			}
			isFinished = true;
			ProcessManager.processManagerSwitch(true);
		}
	}

	private int sumOfElements(ArrayList<Integer> elements) {
		int sum = 0;
		for (int element : elements) {
			sum += element;
		}
		return sum;
	}
}