package com.company.ProcessManager;

import java.util.ArrayList;
import java.util.Random;

import static java.lang.Thread.sleep;

public class Process {
	private static final int longPauseBoundary = 1000;
	private static final int shortPauseBoundary = 10;
	private static final int workBoundary = 1000;
	private static final int intervalsAmountBoundary = 10;
	private static final int priorityLevelsNumber = 10;
	private final ArrayList<Integer> workIntervals = new ArrayList<>();
	private final ArrayList<Integer> pauseIntervals = new ArrayList<>();
	private int priority;
	private volatile int curPriority;
	private int totalDuration;
	private int activeDuration;
	private volatile boolean isFinished;
	private Object resource = null;
	private final Random random;

	public Object getResource() {
		return resource;
	}

	public void setResource(Object resource) {
		this.resource = resource;
	}

	public boolean isFinished() {
		return isFinished;
	}

	public ArrayList<Integer> getWorkIntervals() {
		return workIntervals;
	}

	public int getPriority() {
		return priority;
	}

	public int getCurPriority() {
		return curPriority;
	}

	public void setCurPriority(int prior) {
		curPriority = prior;
	}

	public void setPriority(int priority) {
		this.priority = priority;
		curPriority = priority;
	}

	public int getTotalDuration() {
		return activeDuration + sumOfElements(pauseIntervals);
	}

	public int getActiveDuration() {
		return sumOfElements(workIntervals);
	}

	public Process() {
		random = new Random();
		isFinished = false;
		int amount = random.nextInt(intervalsAmountBoundary);
		for (int i = 0; i < amount; i++) {
			workIntervals.add(random.nextInt(workBoundary));
			pauseIntervals.add(random.nextInt(
					random.nextDouble() > 0.9
							? longPauseBoundary
							: shortPauseBoundary));
		}
		priority = random.nextInt(priorityLevelsNumber);
		curPriority = priority;
	}

	public void run() throws InterruptedException {
		synchronized (this) {
			for (int i = 0; i < workIntervals.size(); i++) {
				sleep(workIntervals.get(i)); // work emulation
				long pauseBeginTime = System.currentTimeMillis();
				do {
					sleep(1);
					if (random.nextInt() % 3 == 0)
						ProcessManager.processManagerSwitch(false);
					else
						ProcessManager.processManagerSwitch(false, resource);
				} while ((System.currentTimeMillis() - pauseBeginTime) < pauseIntervals.get(i)); // I/O emulation*/
			}
			isFinished = true;
			sleep(1);
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