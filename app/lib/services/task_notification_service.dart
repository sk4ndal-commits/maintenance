import 'package:flutter/foundation.dart';

class TaskNotificationService {
  static final ValueNotifier<int> openTaskCount = ValueNotifier<int>(0);

  static void updateTaskCount(int count) {
    openTaskCount.value = count;
  }
}
