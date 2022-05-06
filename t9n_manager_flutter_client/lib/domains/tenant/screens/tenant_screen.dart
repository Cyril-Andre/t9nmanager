import 'package:flutter/material.dart';
import '../../../generated/l10n.dart';

class TenantScreen extends StatefulWidget {
  const TenantScreen({Key? key}) : super(key: key);

  @override
  State<TenantScreen> createState() => _TenantScreenState();
}

class _TenantScreenState extends State<TenantScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(S.of(context).title),
      ),
      body: const Text('Tenant'),
    );
  }
}
